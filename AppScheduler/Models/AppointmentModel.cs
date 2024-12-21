using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppScheduler.Models
{
    public class AppointmentModel
    {
        [Key]
        public int AppointmentId { get; set; }
        public string Date { get; set; }
        public string Details { get; set; }


        public int UserId { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }

        public int ServiceId { get; set; }
        [JsonIgnore]
        public ServiceModel Service { get; set; }

        public int EmployeeId { get; set; }
        [JsonIgnore]
        public EmployeeModel Employee { get; set; }
    }
}
