namespace Employee_Attendance_System.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public ICollection<AttendancePunch> AttendancePunches { get; set; }
    }
}
