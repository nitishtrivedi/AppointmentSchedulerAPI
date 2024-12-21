using System.ComponentModel.DataAnnotations;

namespace AppScheduler.Models
{
    public class EmployeeModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeService {  get; set; }
        public bool isAvailable { get; set; }
        public ICollection<AppointmentModel> Appointments { get; set; }
    }
}
