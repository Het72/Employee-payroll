using System.ComponentModel.DataAnnotations;

namespace EmployeePayrollSystem.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string Position { get; set; } = string.Empty;

    [Range(0, 100000000)]
    public decimal BasicSalary { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}
