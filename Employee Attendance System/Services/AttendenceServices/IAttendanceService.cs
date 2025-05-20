using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Models;

namespace Employee_Attendance_System.Services.AttendenceServices
{
    public interface IAttendanceService
    {
        Task<string> PunchInAsync(PunchRequestDto request);
        Task<string> PunchOutAsync(PunchRequestDto request);
        Task<AttendanceResponseDto> GetTodayAttendance(int employeeId);
        Task<List<AttendanceResponseDto>> GetMonthlyAttendance(int employeeId, int month, int year);
        Task<string> ManualEntryAsync(ManualAttendanceDto dto);
        Task<List<Employee>> GetAbsentEmployees(DateTime date);
    }
}
