namespace Employee_Attendance_System.Models
{
    public class AttendancePunch
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PunchDate { get; set; }
        public TimeSpan? PunchInTime { get; set; }
        public TimeSpan? PunchOutTime { get; set; }
        public string Status { get; set; }
        public Employee Employee { get; set; }
    }
}
