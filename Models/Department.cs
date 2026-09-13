using System.ComponentModel.DataAnnotations;

namespace EmployeePayrollSystem.Models;

public class Department
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
