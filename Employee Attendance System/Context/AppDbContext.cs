using Employee_Attendance_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendance_System.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AttendancePunch> AttendancePunches { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Role)
                .HasDefaultValue("Employee");


            modelBuilder.Entity<AttendancePunch>()
                .HasOne(x => x.Employee)
                .WithMany(e => e.AttendancePunches)
                .HasForeignKey(c => c.EmployeeId);
        }
    }

}
