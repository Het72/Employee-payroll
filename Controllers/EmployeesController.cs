using EmployeePayrollSystem.Data;
using EmployeePayrollSystem.DTOs;
using EmployeePayrollSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public EmployeesController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var query = _context.Employees.AsNoTracking().Include(e => e.Department).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(e => e.FullName.Contains(search) || e.Email.Contains(search) || e.Position.Contains(search));
        }
        var total = await query.CountAsync();
        var employees = await query.OrderBy(e => e.Id).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(e => new EmployeeResponseDto(e.Id, e.FullName, e.Email, e.Phone, e.Position, e.BasicSalary, e.DepartmentId, e.Department!.DepartmentName))
            .ToListAsync();
        return Ok(new { page, pageSize, total, data = employees });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var employee = await _context.Employees.AsNoTracking().Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);
        if (employee is null) return NotFound();
        return Ok(new EmployeeResponseDto(employee.Id, employee.FullName, employee.Email, employee.Phone, employee.Position, employee.BasicSalary, employee.DepartmentId, employee.Department!.DepartmentName));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
            return BadRequest(new { message = "Department does not exist." });
        var email = dto.Email.Trim().ToLowerInvariant();
        if (await _context.Employees.AnyAsync(e => e.Email == email))
            return Conflict(new { message = "Employee email already exists." });
        var employee = new Employee { FullName = dto.FullName.Trim(), Email = email, Phone = dto.Phone.Trim(), Position = dto.Position.Trim(), BasicSalary = dto.BasicSalary, DepartmentId = dto.DepartmentId };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null) return NotFound();
        if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId)) return BadRequest(new { message = "Department does not exist." });
        var email = dto.Email.Trim().ToLowerInvariant();
        if (await _context.Employees.AnyAsync(e => e.Id != id && e.Email == email)) return Conflict(new { message = "Employee email already exists." });
        employee.FullName = dto.FullName.Trim(); employee.Email = email; employee.Phone = dto.Phone.Trim(); employee.Position = dto.Position.Trim(); employee.BasicSalary = dto.BasicSalary; employee.DepartmentId = dto.DepartmentId;
        await _context.SaveChangesAsync();
        return Ok(employee);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null) return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
