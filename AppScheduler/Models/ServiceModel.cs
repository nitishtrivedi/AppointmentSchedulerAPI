using System.ComponentModel.DataAnnotations;

namespace AppScheduler.Models
{
    public class ServiceModel
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
    }
}
