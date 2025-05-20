using Employee_Attendance_System.Api_Response;
using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Services.LoginServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendance_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(new ApiResponse<string>(
                    success: true,
                    message: result,
                    data: null
                   
                ));
            }
            catch (InvalidOperationException ex)  // duplicate email
            {
                return Conflict(new ApiResponse<string>(
                    success: false,
                    message: ex.Message,
                    data: null
                    
                ));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(
                    success: false,
                    message: "Database error during registration.",
                    data: null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(
                    success: false,
                    message: "An unexpected error occurred.",
                    data: null
                ));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            try
            {
                var authResult = await _authService.LoginAsync(dto);
                return Ok(new ApiResponse<AuthResponseDto>(
                    success: true,
                    message: $"Login successful as {authResult.Role}.",
                    data: authResult
                ));
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(new ApiResponse<string>(
                    success: false,
                    message: ex.Message,
                    data: null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>(
                    success: false,
                    message: "An unexpected error occurred during login.",
                    data: null
                ));
            }
        }
    }
}
