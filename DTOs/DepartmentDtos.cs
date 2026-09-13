using System.ComponentModel.DataAnnotations;

namespace EmployeePayrollSystem.DTOs;

public class DepartmentDto
{
    [Required, MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;
}
