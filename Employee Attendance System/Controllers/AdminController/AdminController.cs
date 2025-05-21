using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Services.AdminServices;
using Employee_Attendance_System.Services.AttendenceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Attendance_System.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IAdminService _adminService;

        public AdminController(IAttendanceService attendanceService, IAdminService adminService)
        {
            _attendanceService = attendanceService;
            _adminService = adminService;
        }
        // POST: api/attendance/punch-in
        [HttpPost("punch-in")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [HttpGet("today/{employeeId}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

           // PUT: api/Admin/change-role
        [HttpPut("change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest request)
        {
            var success = await _adminService.ChangeUserRoleAsync(request.UserId, request.NewRole);
            if (!success)
                return NotFound("User not found.");

            return Ok("User role updated successfully.");
        }

        // DELETE: api/Admin/delete-user/{userId}
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var success = await _adminService.DeleteUserAsync(userId);
            if (!success)
                return NotFound("User not found.");

            return Ok("User deleted successfully.");
        }
    }
}

