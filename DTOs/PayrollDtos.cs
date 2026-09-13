using System.ComponentModel.DataAnnotations;

namespace EmployeePayrollSystem.DTOs;

public class PayrollCreateDto
{
    [Range(1, int.MaxValue)] public int EmployeeId { get; set; }
    [Range(0, 100000000)] public decimal Bonus { get; set; }
    [Range(0, 100000000)] public decimal Deduction { get; set; }
    public DateTime SalaryDate { get; set; } = DateTime.UtcNow;
}

public record PayrollResponseDto(
    int Id, int EmployeeId, string EmployeeName, decimal BasicSalary,
    decimal Bonus, decimal Deduction, decimal NetSalary, DateTime SalaryDate);
