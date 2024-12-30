using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AppScheduler.Models
{
    public class AppointmentModel
    {
        [Key]
        public int AppointmentId { get; set; }
        public string Date { get; set; }
        public string Details { get; set; }


        public int UserId { get; set; }
        [JsonProperty]
        public UserModel User { get; set; }

        public int ServiceId { get; set; }
        [JsonProperty]
        public ServiceModel Service { get; set; }

        public int EmployeeId { get; set; }
        [JsonProperty]
        public EmployeeModel Employee { get; set; }
    }
}
