using EmployeePayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayrollSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Payroll> Payrolls => Set<Payroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Employee>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Department>().HasIndex(x => x.DepartmentName).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(x => x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payroll>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.Payrolls)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>().Property(x => x.BasicSalary).HasPrecision(18, 2);
        modelBuilder.Entity<Payroll>().Property(x => x.BasicSalary).HasPrecision(18, 2);
        modelBuilder.Entity<Payroll>().Property(x => x.Bonus).HasPrecision(18, 2);
        modelBuilder.Entity<Payroll>().Property(x => x.Deduction).HasPrecision(18, 2);
    }
}
