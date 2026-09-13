using EmployeePayrollSystem.Data;
using EmployeePayrollSystem.DTOs;
using EmployeePayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayrollSystem.Services;

public class PayrollService : IPayrollService
{
    private readonly ApplicationDbContext _context;

    public PayrollService(ApplicationDbContext context) => _context = context;

    public async Task<PayrollResponseDto?> CreateAsync(PayrollCreateDto dto)
    {
        var employee = await _context.Employees.FindAsync(dto.EmployeeId);
        if (employee is null) return null;

        var payroll = new Payroll
        {
            EmployeeId = employee.Id,
            BasicSalary = employee.BasicSalary,
            Bonus = dto.Bonus,
            Deduction = dto.Deduction,
            SalaryDate = dto.SalaryDate == default ? DateTime.UtcNow : dto.SalaryDate
        };

        _context.Payrolls.Add(payroll);
        await _context.SaveChangesAsync();

        return new PayrollResponseDto(
            payroll.Id, employee.Id, employee.FullName, payroll.BasicSalary,
            payroll.Bonus, payroll.Deduction, payroll.NetSalary, payroll.SalaryDate);
    }
}
