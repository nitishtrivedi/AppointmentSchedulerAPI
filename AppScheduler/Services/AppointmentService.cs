using AppScheduler.Data;
using AppScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace AppScheduler.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentModel> CreateAppointmentAsync(int userId, int serviceId, string appointmentDate);
        Task<AppointmentModel> GetAppointmentAsync(int appointmentId);
    }
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDBContext _context;
        private readonly Random _random;

        public AppointmentService(AppDBContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task<AppointmentModel> CreateAppointmentAsync(int userId, int serviceId, string appointmentDate)
        {
            //GET USERS, SERVICES AND AVAILABLE EMPLOYEES
            var user = await _context.Users.FindAsync(userId);
            var service = await _context.Services.FindAsync(serviceId);

            if (user == null || service == null)
            {
                throw new ArgumentException("Invalid User or Service");
            }

            //Get list of available employees
            var availableEmployees = await _context.Employees
                .Where(e => e.isAvailable)
                .Where(e => e.EmployeeService == service.ServiceName)
                .ToListAsync();

            if (availableEmployees.Count == 0)
            {
                throw new InvalidOperationException("No employees available");
            }

            //Randomly select an employee
            var randomEmployee = availableEmployees[_random.Next(availableEmployees.Count)];

            //Create a new appointment
            var appointment = new AppointmentModel
            {
                UserId = userId,
                ServiceId = serviceId,
                EmployeeId = randomEmployee.EmployeeId,
                Date = appointmentDate
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            //Set employee as not available once appointment is created
            randomEmployee.isAvailable = false;
            _context.Entry(randomEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return appointment;
        }

        public async Task<AppointmentModel> GetAppointmentAsync(int appointmentId)
        {
            return await _context.Appointments
            .Include(a => a.User)
            .Include(a => a.Service)
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }
    }
}
