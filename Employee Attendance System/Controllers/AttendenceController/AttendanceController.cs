using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Services.AttendenceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Attendance_System.Controllers.AttendenceController
{

    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // POST: api/attendance/punch-in
        [HttpPost("punch-in")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PunchIn([FromBody] PunchRequestDto request)
        {
            try
            {
                var result = await _attendanceService.PunchInAsync(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/attendance/punch-out
        [HttpPost("punch-out")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PunchOut([FromBody] PunchRequestDto request)
        {
            try
            {
                var result = await _attendanceService.PunchOutAsync(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/attendance/today/{employeeId}
        [HttpGet("today-B/{employeeId}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> GetTodayAttendance(int employeeId)
        {
            try
            {
                var result = await _attendanceService.GetTodayAttendance(employeeId);
                if (result == null)
                    return NotFound(new { message = "No attendance record found for today." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/attendance/monthly?employeeId=1&month=5&year=2025
        [HttpGet("monthly -B")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> GetMonthlyAttendance(int employeeId, int month, int year)
        {
            try
            {
                var result = await _attendanceService.GetMonthlyAttendance(employeeId, month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/attendance/manual-entry
        [HttpPost("manual-entry")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManualEntry([FromBody] ManualAttendanceDto dto)
        {
            try
            {
                var result = await _attendanceService.ManualEntryAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/attendance/absent?date=2025-05-19
        [HttpGet("absent")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAbsentEmployees([FromQuery] DateTime date)
        {
            try
            {
                var result = await _attendanceService.GetAbsentEmployees(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
