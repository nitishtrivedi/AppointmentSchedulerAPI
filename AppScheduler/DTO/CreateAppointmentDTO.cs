namespace AppScheduler.DTO
{
    public class CreateAppointmentDTO
    {
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public string AppointmentDate { get; set; }
    }
}
