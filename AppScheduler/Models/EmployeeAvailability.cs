using System.ComponentModel.DataAnnotations;

namespace AppScheduler.Models
{
    public class EmployeeAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public int ServiceId { get; set; }
        public ServiceModel Service { get; set; }
        public string Date { get; set; }
        public bool IsBooked { get; set; }
    }


    // Helper class for checking availability
    public class AvailabilityRequest
    {
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public string Date { get; set; }
    }

    public class AvailabilityResponse
    {
        public bool IsAvailable { get; set; }
        public string Message { get; set; }
    }
}
