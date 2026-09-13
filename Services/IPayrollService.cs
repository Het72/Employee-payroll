using EmployeePayrollSystem.DTOs;

namespace EmployeePayrollSystem.Services;

public interface IPayrollService
{
    Task<PayrollResponseDto?> CreateAsync(PayrollCreateDto dto);
}
