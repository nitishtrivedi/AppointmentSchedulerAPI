using System.ComponentModel.DataAnnotations;

namespace AppScheduler.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public bool IsUserAdmin { get; set; }
        public ICollection<AppointmentModel> Appointments { get; set; }
    }
}
