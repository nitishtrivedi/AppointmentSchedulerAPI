using AppScheduler.DTO;
using AppScheduler.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentsService;
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentsService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO appointmentDTO)
        {
            try
            {
                var appointment = await _appointmentsService.CreateAppointmentAsync(appointmentDTO.UserId, appointmentDTO.ServiceId, appointmentDTO.AppointmentDate);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _appointmentsService.GetAppointmentAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }
    }
}
