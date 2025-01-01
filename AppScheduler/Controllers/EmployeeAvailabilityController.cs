using AppScheduler.Models;
using AppScheduler.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeAvailabilityController : ControllerBase
    {
        private readonly EmployeeAvailabilityService _availabilityService;
        public EmployeeAvailabilityController(EmployeeAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost("check")]
        public async Task<ActionResult<AvailabilityResponse>> CheckAvailability([FromBody] AvailabilityRequest request)
        {
            try
            {
                var result = await _availabilityService.CheckEmployeeAvailability(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking availability", error = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateAvailability([FromBody] UpdateAvailabilityRequest request)
        {
            try
            {
                var result = await _availabilityService.UpdateEmployeeAvailability(
                    request.EmployeeId,
                    request.ServiceId,
                    request.Date,
                    request.IsAvailable
                );

                if (!result)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                return Ok(new { message = "Availability updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating availability", error = ex.Message });
            }
        }
    }

    public class UpdateAvailabilityRequest
    {
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public string Date { get; set; }
        public bool IsAvailable { get; set; }
    }
}
