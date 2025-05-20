using Employee_Attendance_System.Context;
using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace Employee_Attendance_System.Services.LoginServices
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            try
            {
                if (await _context.Employees.AnyAsync(e => e.Email == dto.Email))
                    throw new InvalidOperationException($"An account with email '{dto.Email}' already exists.");

                var employee = new Employee
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                  
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return "Registration successful.";
            }
            catch (DbUpdateException dbEx)
            {
                
                throw new ApplicationException("A database error occurred while registering.", dbEx);
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            try
            {
              
                var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == dto.Email);

              
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                    throw new AuthenticationException("Invalid email or password.");

               
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role),
                    new Claim(ClaimTypes.Name,user.Name)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                );

                return new AuthResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Role = user.Role,
                    Name = user.Name
                };
            }
            catch (AuthenticationException)
            {
                // re‑throw to be handled by the controller
                throw;
            }
            catch (Exception ex)
            {
                // Wrap any other exception
                throw new ApplicationException("An unexpected error occurred during login.", ex);
            }
        }
    }
}
