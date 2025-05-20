using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Services.AttendenceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Attendance_System.Controllers.EmployeeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public EmployeeController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        // POST: api/attendance/punch-in
        [HttpPost("punch-in")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PunchIn()
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var result = await _attendanceService.PunchInAsync(employeeId);
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
        public async Task<IActionResult> PunchOut()
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var result = await _attendanceService.PunchOutAsync(employeeId);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // GET: api/attendance/today/{employeeId}
        [HttpGet("today")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetToday()
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var result = await _attendanceService.GetToday(employeeId);
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
        [HttpGet("monthly")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMonthlyAttendance(int month, int year)
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var result = await _attendanceService.GetMonthlyAttendance(employeeId, month, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // GET: api/attendance/absent?date=2025-05-19
        [HttpGet("absent-check")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> IsEmployeeAbsent([FromQuery] string date)
        {
            try
            {
                int employeeId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var isAbsent = await _attendanceService.IsEmployeeAbsent(employeeId, date);
                return Ok(new { date, isAbsent });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
