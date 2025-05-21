namespace Employee_Attendance_System.Services.AdminServices
{
    public interface IAdminService
    {
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ChangeUserRoleAsync(int userId, string newRole);

    }
}
