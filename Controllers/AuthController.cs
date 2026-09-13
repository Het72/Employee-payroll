using BCrypt.Net;
using EmployeePayrollSystem.Data;
using EmployeePayrollSystem.DTOs;
using EmployeePayrollSystem.Models;
using EmployeePayrollSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayrollSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(x => x.Email == email))
            return Conflict(new { message = "Email already exists." });

        var user = new User
        {
            Username = dto.Username.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Employee"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created("/api/auth/register", new
        {
            message = "User registered successfully.",
            user.Id,
            user.Username,
            user.Email,
            user.Role
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(new
        {
            token = _jwtService.GenerateToken(user),
            expiresInMinutes = 60,
            user = new { user.Id, user.Username, user.Email, user.Role }
        });
    }
}
