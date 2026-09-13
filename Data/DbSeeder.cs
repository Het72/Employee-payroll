using BCrypt.Net;
using EmployeePayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayrollSystem.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Departments.AnyAsync())
        {
            db.Departments.AddRange(
                new Department { DepartmentName = "Human Resources" },
                new Department { DepartmentName = "Engineering" },
                new Department { DepartmentName = "Finance" });
            await db.SaveChangesAsync();
        }

        if (!await db.Users.AnyAsync())
        {
            db.Users.Add(new User
            {
                Username = "admin",
                Email = "admin@payroll.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@12345"),
                Role = "Admin"
            });
            await db.SaveChangesAsync();
        }
    }
}
