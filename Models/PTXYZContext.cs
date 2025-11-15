using System;
using System.Data.Entity;
using System.Linq;

namespace PTXYZ_OvertimeApp.Models
{
    public class PTXYZContext : DbContext
    {
        public PTXYZContext() : base("name=PTXYZContext")
        {
            Database.SetInitializer(new PTXYZDbInitializer());
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OverTime> OverTimes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Employee>()
                .HasRequired(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OverTime>()
                .HasRequired(o => o.Employee)
                .WithMany(e => e.OverTimes)
                .HasForeignKey(o => o.EmployeeId)
                .WillCascadeOnDelete(false);
        }
    }

    // Database Initializer with seed data
    public class PTXYZDbInitializer : CreateDatabaseIfNotExists<PTXYZContext>
    {
        protected override void Seed(PTXYZContext context)
        {
            // Seed Departments
            var departments = new[]
            {
                new Department { DepartmentName = "Production", Description = "Production Department" },
                new Department { DepartmentName = "Engineering", Description = "Engineering Department" },
                new Department { DepartmentName = "Quality Control", Description = "Quality Control Department" },
                new Department { DepartmentName = "Maintenance", Description = "Maintenance Department" },
                new Department { DepartmentName = "Human Resources", Description = "HR Department" },
                new Department { DepartmentName = "Finance", Description = "Finance Department" },
                new Department { DepartmentName = "Information Technology", Description = "IT Department" }
            };

            foreach (var dept in departments)
            {
                context.Departments.Add(dept);
            }
            context.SaveChanges();

            // Seed Employees
            var employees = new[]
            {
                new Employee { NIK = "EMP001", FullName = "John Anderson", DepartmentId = 1, Position = "Manager", LaptopAllowance = true, MealAllowance = false, Address = "Jakarta Selatan", PhoneNumber = "081234567890", JoinDate = new DateTime(2020, 1, 15) },
                new Employee { NIK = "EMP002", FullName = "Sarah Wilson", DepartmentId = 1, Position = "Supervisor", LaptopAllowance = true, MealAllowance = true, Address = "Jakarta Timur", PhoneNumber = "081234567891", JoinDate = new DateTime(2020, 3, 20) },
                new Employee { NIK = "EMP003", FullName = "Michael Brown", DepartmentId = 2, Position = "Leader", LaptopAllowance = false, MealAllowance = true, Address = "Bekasi", PhoneNumber = "081234567892", JoinDate = new DateTime(2021, 2, 10) },
                new Employee { NIK = "EMP004", FullName = "Emily Davis", DepartmentId = 2, Position = "Technician", LaptopAllowance = false, MealAllowance = true, Address = "Tangerang", PhoneNumber = "081234567893", JoinDate = new DateTime(2021, 5, 5) },
                new Employee { NIK = "EMP005", FullName = "David Martinez", DepartmentId = 3, Position = "Operator", LaptopAllowance = false, MealAllowance = true, Address = "Bogor", PhoneNumber = "081234567894", JoinDate = new DateTime(2022, 1, 20) },
                new Employee { NIK = "EMP006", FullName = "Jessica Taylor", DepartmentId = 3, Position = "Supervisor", LaptopAllowance = true, MealAllowance = true, Address = "Jakarta Barat", PhoneNumber = "081234567895", JoinDate = new DateTime(2021, 8, 15) },
                new Employee { NIK = "EMP007", FullName = "James Johnson", DepartmentId = 4, Position = "Technician", LaptopAllowance = false, MealAllowance = true, Address = "Depok", PhoneNumber = "081234567896", JoinDate = new DateTime(2022, 3, 10) },
                new Employee { NIK = "EMP008", FullName = "Linda Garcia", DepartmentId = 5, Position = "Manager", LaptopAllowance = true, MealAllowance = false, Address = "Jakarta Pusat", PhoneNumber = "081234567897", JoinDate = new DateTime(2019, 6, 1) },
                new Employee { NIK = "EMP009", FullName = "Robert Rodriguez", DepartmentId = 6, Position = "Supervisor", LaptopAllowance = true, MealAllowance = true, Address = "Jakarta Utara", PhoneNumber = "081234567898", JoinDate = new DateTime(2020, 9, 25) },
                new Employee { NIK = "EMP010", FullName = "Maria Hernandez", DepartmentId = 7, Position = "Leader", LaptopAllowance = false, MealAllowance = true, Address = "Bekasi Timur", PhoneNumber = "081234567899", JoinDate = new DateTime(2021, 11, 12) },
                new Employee { NIK = "EMP011", FullName = "William Moore", DepartmentId = 1, Position = "Operator", LaptopAllowance = false, MealAllowance = true, Address = "Tangerang Selatan", PhoneNumber = "081234567800", JoinDate = new DateTime(2022, 6, 20) },
                new Employee { NIK = "EMP012", FullName = "Patricia Lee", DepartmentId = 2, Position = "Operator", LaptopAllowance = false, MealAllowance = true, Address = "Jakarta Selatan", PhoneNumber = "081234567801", JoinDate = new DateTime(2022, 8, 5) },
                new Employee { NIK = "EMP013", FullName = "Christopher Walker", DepartmentId = 4, Position = "Leader", LaptopAllowance = false, MealAllowance = true, Address = "Bogor Barat", PhoneNumber = "081234567802", JoinDate = new DateTime(2021, 4, 18) },
                new Employee { NIK = "EMP014", FullName = "Barbara Hall", DepartmentId = 3, Position = "Technician", LaptopAllowance = false, MealAllowance = true, Address = "Depok Timur", PhoneNumber = "081234567803", JoinDate = new DateTime(2022, 2, 28) },
                new Employee { NIK = "EMP015", FullName = "Daniel Allen", DepartmentId = 7, Position = "Technician", LaptopAllowance = false, MealAllowance = true, Address = "Jakarta Timur", PhoneNumber = "081234567804", JoinDate = new DateTime(2022, 5, 15) }
            };

            foreach (var emp in employees)
            {
                context.Employees.Add(emp);
            }
            context.SaveChanges();

            // Seed Overtime entries
            var overtimes = new[]
            {
                new OverTime { EmployeeId = 1, Date = new DateTime(2024, 10, 1), TimeStart = new DateTime(2024, 10, 1, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 1, 19, 30, 0), ActualOTHours = 2.5m, CalculatedOTHours = 5.0m, Remarks = "Project deadline" },
                new OverTime { EmployeeId = 2, Date = new DateTime(2024, 10, 2), TimeStart = new DateTime(2024, 10, 2, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 2, 20, 0, 0), ActualOTHours = 3.0m, CalculatedOTHours = 6.0m, Remarks = "Production support" },
                new OverTime { EmployeeId = 3, Date = new DateTime(2024, 10, 3), TimeStart = new DateTime(2024, 10, 3, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 3, 18, 30, 0), ActualOTHours = 1.5m, CalculatedOTHours = 3.0m, Remarks = "System maintenance" },
                new OverTime { EmployeeId = 4, Date = new DateTime(2024, 10, 5), TimeStart = new DateTime(2024, 10, 5, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 5, 19, 0, 0), ActualOTHours = 2.0m, CalculatedOTHours = 4.0m, Remarks = "Equipment repair" },
                new OverTime { EmployeeId = 5, Date = new DateTime(2024, 10, 7), TimeStart = new DateTime(2024, 10, 7, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 7, 18, 0, 0), ActualOTHours = 1.0m, CalculatedOTHours = 2.0m, Remarks = "Quality inspection" },
                new OverTime { EmployeeId = 6, Date = new DateTime(2024, 10, 8), TimeStart = new DateTime(2024, 10, 8, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 8, 20, 0, 0), ActualOTHours = 3.0m, CalculatedOTHours = 6.0m, Remarks = "Urgent quality check" },
                new OverTime { EmployeeId = 7, Date = new DateTime(2024, 10, 10), TimeStart = new DateTime(2024, 10, 10, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 10, 19, 30, 0), ActualOTHours = 2.5m, CalculatedOTHours = 5.0m, Remarks = "Machine troubleshooting" },
                new OverTime { EmployeeId = 2, Date = new DateTime(2024, 10, 12), TimeStart = new DateTime(2024, 10, 12, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 12, 18, 30, 0), ActualOTHours = 1.5m, CalculatedOTHours = 3.0m, Remarks = "Team meeting" },
                new OverTime { EmployeeId = 10, Date = new DateTime(2024, 10, 14), TimeStart = new DateTime(2024, 10, 14, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 14, 19, 0, 0), ActualOTHours = 2.0m, CalculatedOTHours = 4.0m, Remarks = "System upgrade" },
                new OverTime { EmployeeId = 11, Date = new DateTime(2024, 10, 15), TimeStart = new DateTime(2024, 10, 15, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 15, 18, 30, 0), ActualOTHours = 1.5m, CalculatedOTHours = 3.0m, Remarks = "Production line support" },
                new OverTime { EmployeeId = 13, Date = new DateTime(2024, 10, 18), TimeStart = new DateTime(2024, 10, 18, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 18, 20, 0, 0), ActualOTHours = 3.0m, CalculatedOTHours = 6.0m, Remarks = "Emergency maintenance" },
                new OverTime { EmployeeId = 14, Date = new DateTime(2024, 10, 20), TimeStart = new DateTime(2024, 10, 20, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 20, 19, 30, 0), ActualOTHours = 2.5m, CalculatedOTHours = 5.0m, Remarks = "Quality audit preparation" },
                new OverTime { EmployeeId = 3, Date = new DateTime(2024, 10, 22), TimeStart = new DateTime(2024, 10, 22, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 22, 18, 0, 0), ActualOTHours = 1.0m, CalculatedOTHours = 2.0m, Remarks = "Documentation update" },
                new OverTime { EmployeeId = 9, Date = new DateTime(2024, 10, 25), TimeStart = new DateTime(2024, 10, 25, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 25, 19, 0, 0), ActualOTHours = 2.0m, CalculatedOTHours = 4.0m, Remarks = "Budget review" },
                new OverTime { EmployeeId = 15, Date = new DateTime(2024, 10, 28), TimeStart = new DateTime(2024, 10, 28, 17, 0, 0), TimeFinish = new DateTime(2024, 10, 28, 19, 30, 0), ActualOTHours = 2.5m, CalculatedOTHours = 5.0m, Remarks = "Network maintenance" }
            };

            foreach (var ot in overtimes)
            {
                context.OverTimes.Add(ot);
            }
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
