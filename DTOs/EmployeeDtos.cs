using System.ComponentModel.DataAnnotations;

namespace EmployeePayrollSystem.DTOs;

public class EmployeeCreateDto
{
    [Required, MaxLength(120)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(120)] public string Email { get; set; } = string.Empty;
    [MaxLength(20)] public string Phone { get; set; } = string.Empty;
    [Required, MaxLength(80)] public string Position { get; set; } = string.Empty;
    [Range(0, 100000000)] public decimal BasicSalary { get; set; }
    [Range(1, int.MaxValue)] public int DepartmentId { get; set; }
}

public class EmployeeUpdateDto : EmployeeCreateDto { }

public record EmployeeResponseDto(
    int Id, string FullName, string Email, string Phone, string Position,
    decimal BasicSalary, int DepartmentId, string DepartmentName);
