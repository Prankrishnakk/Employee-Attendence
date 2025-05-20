namespace Employee_Attendance_System.Dto
{
    public class ManualAttendanceDto
    {
        public int EmployeeId { get; set; }
        public DateTime PunchDate { get; set; }
        public TimeSpan? PunchInTime { get; set; }
        public TimeSpan? PunchOutTime { get; set; }
        public string Status { get; set; }
    }
}
