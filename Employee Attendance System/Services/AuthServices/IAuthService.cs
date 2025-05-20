using Employee_Attendance_System.Dto;

namespace Employee_Attendance_System.Services.LoginServices
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
