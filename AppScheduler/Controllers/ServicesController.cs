using AppScheduler.Data;
using AppScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ServicesController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddService(ServiceModel service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return Ok(service);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound("Service Not Found");
            return Ok(service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditService(ServiceModel service, int id)
        {
            if (service.ServiceId != id) service.ServiceId = id;
            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                if (!_context.Services.Any(e => e.ServiceId == id)) return NotFound("Service Not Found");
                throw;
            }
            return Ok(service);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound("Service Not Found");
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
