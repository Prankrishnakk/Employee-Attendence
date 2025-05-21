using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Services.AttendenceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Attendance_System.Controllers.SuperAdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public SuperAdminController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
       
        // GET: api/attendance/today/{employeeId}
        [HttpGet("today/{employeeId}")]
        [Authorize(Roles = "SuperAdmin")]
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
        [HttpGet("monthly")]
        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAbsentEmployees([FromQuery] string date)
        {
            try
            {
                var result = await _attendanceService.GetAbsentEmployees(date);
                return Ok(result);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(new { error = argEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
