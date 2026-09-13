using EmployeePayrollSystem.Models;

namespace EmployeePayrollSystem.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
