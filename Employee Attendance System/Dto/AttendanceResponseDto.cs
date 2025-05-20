namespace Employee_Attendance_System.Dto
{
    public class AttendanceResponseDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public TimeSpan? PunchInTime { get; set; }
        public TimeSpan? PunchOutTime { get; set; }
    }
}
