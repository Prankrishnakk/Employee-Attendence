using Employee_Attendance_System.Context;
using Employee_Attendance_System.Dto;
using Employee_Attendance_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendance_System.Services.AttendenceServices
{
    public class AttendanceService : IAttendanceService
    {
        private readonly AppDbContext _context;

        public AttendanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> PunchInAsync(int employeeId)
        {
            try
            {
                var today = DateTime.Today;
                var now = DateTime.Now.TimeOfDay;

                var existingPunch = await _context.AttendancePunches
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.PunchDate == today);

                if (existingPunch != null)
                    return "Already punched in today.";

                string status = now > new TimeSpan(9, 15, 0) ? "Late" : "Present";

                var punch = new AttendancePunch
                {
                    EmployeeId = employeeId,
                    PunchDate = today,
                    PunchInTime = now,
                    Status = status
                };

                _context.AttendancePunches.Add(punch);
                await _context.SaveChangesAsync();

                return "Punch in successful.";
            }
            catch (Exception ex)
            {
                return $"Error during punch in: {ex.Message}";
            }
        }


        public async Task<string> PunchOutAsync(int employeeId)
        {
            try
            {
                var today = DateTime.Today;
                var now = DateTime.Now.TimeOfDay;

                var punch = await _context.AttendancePunches
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.PunchDate == today);

                if (punch == null)
                    return "No punch in record found for today.";

                if (punch.PunchOutTime != null)
                    return "Already punched out today.";

                if (now < new TimeSpan(17, 0, 0))
                    punch.Status = "Early Leave";

                punch.PunchOutTime = now;
                await _context.SaveChangesAsync();

                return "Punch out successful.";
            }
            catch (Exception ex)
            {
                return $"Error during punch out: {ex.Message}";
            }
        }


        public async Task<AttendanceResponseDto> GetTodayAttendance(int employeeId)
        {
            try
            {
                var today = DateTime.Today;
                var punch = await _context.AttendancePunches
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.PunchDate == today);

                if (punch == null) return null;

                return new AttendanceResponseDto
                {
                    Date = today,
                    Status = punch.Status,
                    PunchInTime = punch.PunchInTime,
                    PunchOutTime = punch.PunchOutTime
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch today's attendance.");
            }
        }

        public async Task<List<AttendanceResponseDto>> GetMonthlyAttendance(int employeeId, int month, int year)
        {
            try
            {
                return await _context.AttendancePunches
                    .Where(x => x.EmployeeId == employeeId && x.PunchDate.Month == month && x.PunchDate.Year == year)
                    .Select(x => new AttendanceResponseDto
                    {
                        Date = x.PunchDate,
                        Status = x.Status,
                        PunchInTime = x.PunchInTime,
                        PunchOutTime = x.PunchOutTime
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch monthly attendance.");
            }
        }

        public async Task<string> ManualEntryAsync(ManualAttendanceDto dto)
        {
            try
            {
                var punch = await _context.AttendancePunches
                    .FirstOrDefaultAsync(x => x.EmployeeId == dto.EmployeeId && x.PunchDate == dto.PunchDate);

                if (punch == null)
                {
                    punch = new AttendancePunch
                    {
                        EmployeeId = dto.EmployeeId,
                        PunchDate = dto.PunchDate,
                        PunchInTime = dto.PunchInTime,
                        PunchOutTime = dto.PunchOutTime,
                        Status = dto.Status
                    };
                    _context.AttendancePunches.Add(punch);
                }
                else
                {
                    punch.PunchInTime = dto.PunchInTime;
                    punch.PunchOutTime = dto.PunchOutTime;
                    punch.Status = dto.Status;
                }

                await _context.SaveChangesAsync();
                return "Manual entry updated successfully.";
            }
            catch (Exception ex)
            {
                return $"Error during manual entry: {ex.Message}";
            }
        }

        public async Task<List<Employee>> GetAbsentEmployees(string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                throw new ArgumentException("Invalid date format. Please use 'yyyy-MM-dd' or a recognizable date.");

            try
            {
                var attendedEmployeeIds = await _context.AttendancePunches
                    .Where(x => x.PunchDate.Date == parsedDate.Date)
                    .Select(x => x.EmployeeId)
                    .ToListAsync();

                var absentEmployees = await _context.Employees
                    .Where(x => !attendedEmployeeIds.Contains(x.Id))
                    .ToListAsync();

                return absentEmployees;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch absent employees.", ex);
            }
        }
        public async Task<bool> IsEmployeeAbsent(int employeeId, string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                throw new ArgumentException("Invalid date format. Please use 'yyyy-MM-dd' or a recognizable date.");

            try
            {
                var hasAttendance = await _context.AttendancePunches
                    .AnyAsync(x => x.EmployeeId == employeeId && x.PunchDate.Date == parsedDate.Date);

                return !hasAttendance;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to check employee attendance.", ex);
            }
        }
        public async Task<AttendanceResponseDto> GetToday(int employeeId)
        {
            try
            {
                var today = DateTime.Today;
                var punch = await _context.AttendancePunches
                    .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.PunchDate == today);

                if (punch == null) return null;

                return new AttendanceResponseDto
                {
                    Date = today,
                    Status = punch.Status,
                    PunchInTime = punch.PunchInTime,
                    PunchOutTime = punch.PunchOutTime
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to fetch today's attendance.");
            }
        }
    }
}
