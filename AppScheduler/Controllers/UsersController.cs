using AppScheduler.Data;
using AppScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDBContext _context;
        public UsersController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _context.Users.Include(u => u.Appointments).Select(u => new
            {
                u.UserId,
                u.UserName,
                u.UserFirstName,
                u.UserLastName,
                u.UserPassword,
                u.UserEmail,
                u.UserPhone,
                u.UserAddress,
                u.IsUserAdmin,
                Appointments = u.Appointments.Select(a => new {
                    a.AppointmentId,
                }).ToList()
            }).ToListAsync();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Appointments).Select(u => new
            {
                u.UserId,
                u.UserName,
                u.UserFirstName,
                u.UserLastName,
                u.UserPassword,
                u.UserEmail,
                u.UserPhone,
                u.UserAddress,
                u.IsUserAdmin,
                Appointments = u.Appointments.Select(a => new {
                    a.AppointmentId,
                }).ToList()
            }).FirstOrDefaultAsync(e => e.UserId == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserModel user)
        {
            if (id != user.UserId) user.UserId = id;
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!_context.Users.Any(e => e.UserId == id)) return NotFound();
                throw;
            }
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User Deleted");
        }
    }
}
