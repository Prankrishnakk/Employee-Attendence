using Employee_Attendance_System.Context;

namespace Employee_Attendance_System.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeUserRoleAsync(int userId, string newRole)
        {
            var user = await _context.Employees.FindAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Employees.FindAsync(userId);
            if (user == null) return false;

            _context.Employees.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
