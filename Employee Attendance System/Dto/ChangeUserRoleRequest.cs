namespace Employee_Attendance_System.Dto
{
    public class ChangeUserRoleRequest
    {
        public int UserId { get; set; }
        public string NewRole { get; set; }
    }
}
