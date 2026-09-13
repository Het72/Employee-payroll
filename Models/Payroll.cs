using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePayrollSystem.Models;

public class Payroll
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [Range(0, 100000000)]
    public decimal BasicSalary { get; set; }

    [Range(0, 100000000)]
    public decimal Bonus { get; set; }

    [Range(0, 100000000)]
    public decimal Deduction { get; set; }

    public DateTime SalaryDate { get; set; }

    [NotMapped]
    public decimal NetSalary => BasicSalary + Bonus - Deduction;
}
