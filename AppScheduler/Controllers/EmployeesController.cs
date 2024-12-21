using AppScheduler.Data;
using AppScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeesController : ControllerBase
    {
        private readonly AppDBContext _context;
        public EmployeesController(AppDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employee = await _context.Employees.Include(e => e.Appointments).Select(u => new
            {
                u.EmployeeId,
                u.EmployeeName,
                u.EmployeeService,
                u.isAvailable,
                Appointment = u.Appointments.Select(a => new
                {
                    a.AppointmentId
                }).ToList()
            }).ToListAsync();
            return Ok(employee);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.Include(e => e.Appointments).Select(u => new
            {
                u.EmployeeId,
                u.EmployeeName,
                u.EmployeeService,
                u.isAvailable,
                Appointment = u.Appointments.Select(a => new
                {
                    a.AppointmentId
                }).ToList()
            }).FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeModel employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployee(EmployeeModel employee, int id)
        {
            if (employee.EmployeeId != id) employee.EmployeeId = id;
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.EmployeeId == id)) return NotFound();
                throw;
            }
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
