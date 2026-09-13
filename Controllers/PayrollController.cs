using EmployeePayrollSystem.Data;
using EmployeePayrollSystem.DTOs;
using EmployeePayrollSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EmployeePayrollSystem.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class PayrollController : ControllerBase 
{
    private readonly ApplicationDbContext _context;
    private readonly IPayrollService _payrollService;
    public PayrollController(ApplicationDbContext context, 
        IPayrollService payrollService) 
    {
        _context = context;
        _payrollService = payrollService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? employeeId,
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var query = _context.Payrolls.AsNoTracking().Include(p => p.Employee).AsQueryable(); 
        if (employeeId.HasValue) 
        {
            query = query.Where(p => p.EmployeeId == employeeId.Value); 
        }
        var total = await query.CountAsync(); 
        var data = await query.OrderByDescending(p => p.SalaryDate).Skip((page - 1) * pageSize).Take(pageSize).Select(p => new PayrollResponseDto(p.Id, p.EmployeeId, p.Employee!.FullName, p.BasicSalary, p.Bonus, p.Deduction, p.BasicSalary + p.Bonus - p.Deduction, p.SalaryDate)).ToListAsync(); 
        return Ok(new 
        {
            page, pageSize, total, data 
        }); 
    }
    [HttpGet("{id:int}")] 
    public async Task<IActionResult> Get(int id) 
    {
        var payroll = await _context.Payrolls.AsNoTracking().Include(p => p.Employee).FirstOrDefaultAsync(p => p.Id == id); if (payroll is null) { return NotFound();
        }
        return Ok(new PayrollResponseDto(payroll.Id, payroll.EmployeeId, payroll.Employee!.FullName, payroll.BasicSalary, payroll.Bonus, payroll.Deduction, payroll.NetSalary, payroll.SalaryDate)); 
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(PayrollCreateDto dto) 
    {
        var result = await _payrollService.CreateAsync(dto);
        if (result is null) 
        {
            return NotFound(new { message = "Employee not found." });
        }
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result); 
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) 
    {
        var payroll = await _context.Payrolls.FindAsync(id);
        if (payroll is null) 
        {
            return NotFound();
        }
        _context.Payrolls.Remove(payroll);
        await _context.SaveChangesAsync();
        return NoContent(); } }