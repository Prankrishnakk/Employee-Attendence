using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Models;

namespace Employee_Attendance_System.Services.AttendenceServices
{
    public interface IAttendanceService
    {
        Task<string> PunchInAsync(int employeeId);
        Task<string> PunchOutAsync(int employeeId);
        Task<AttendanceResponseDto> GetTodayAttendance(int employeeId);
        Task<List<AttendanceResponseDto>> GetMonthlyAttendance(int employeeId, int month, int year);
        Task<string> ManualEntryAsync(ManualAttendanceDto dto);
        Task<List<Employee>> GetAbsentEmployees(string date);
        Task<bool> IsEmployeeAbsent(int employeeId, string date);
        Task<AttendanceResponseDto> GetToday(int employeeId);
    }
}
