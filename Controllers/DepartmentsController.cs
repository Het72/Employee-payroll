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
public class DepartmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DepartmentsController(ApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Departments
        .AsNoTracking().Select(d => new { d.Id, d.DepartmentName, EmployeeCount = d.Employees.Count }).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var department = await _context.Departments.AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new { d.Id, d.DepartmentName, Employees = d.Employees.Select(e => new { e.Id, e.FullName, e.Email, e.Position }) })
            .FirstOrDefaultAsync();
        return department is null ? NotFound() : Ok(department);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(DepartmentDto dto)
    {
        var name = dto.DepartmentName.Trim();
        if (await _context.Departments.AnyAsync(d => d.DepartmentName == name))
            return Conflict(new { message = "Department already exists." });
        var department = new Department { DepartmentName = name };
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = department.Id }, department);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null) return NotFound();
        var name = dto.DepartmentName.Trim();
        if (await _context.Departments.AnyAsync(d => d.Id != id && d.DepartmentName == name))
            return Conflict(new { message = "Department already exists." });
        department.DepartmentName = name;
        await _context.SaveChangesAsync();
        return Ok(department);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _context.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
        if (department is null) return NotFound();
        if (department.Employees.Any()) return Conflict(new { message = "Cannot delete a department that has employees." });
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
