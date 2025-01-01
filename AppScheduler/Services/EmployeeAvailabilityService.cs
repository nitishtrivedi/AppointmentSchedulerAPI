using AppScheduler.Data;
using AppScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace AppScheduler.Services
{
    public class EmployeeAvailabilityService
    {
        private readonly AppDBContext _context;
        public EmployeeAvailabilityService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<AvailabilityResponse> CheckEmployeeAvailability(AvailabilityRequest request)
        {
            var response = new AvailabilityResponse();

            //GET EMPLOYEE AVAILABILITY FOR THE DATE
            var availability = await _context.EmployeeAvailability
                .Where(a => a.EmployeeId == request.EmployeeId &&
                a.ServiceId == request.ServiceId &&
                a.Date == request.Date).FirstOrDefaultAsync();

            //GET EXISTING APPOINTMENTS
            var hasExistingAppointments = await _context.Appointments
                .Where(a => a.EmployeeId == request.EmployeeId &&
                           a.Date == request.Date)
                .AnyAsync();

            if (availability != null)
            {
                response.Message = "Employee is not available on this date";
                return response;
            }

            if (hasExistingAppointments)
            {
                response.Message = "Employee is already booked for this date";
                return response;
            }
            // If no availability record exists, assume the employee is available
            if (availability == null)
            {
                response.IsAvailable = true;
                response.Message = "Employee is available";
                return response;
            }

            // If availability record exists, use its IsBooked property

            response.IsAvailable = !availability.IsBooked;
            response.Message = response.IsAvailable
                ? "Employee is available"
                : "Employee is not available on this date";

            return response;
        }

        public async Task<bool> UpdateEmployeeAvailability(int employeeId, int serviceId, string date, bool isAvailable)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return false;

            // Update the employee's availability flag
            employee.isAvailable = isAvailable;

            // Check if availability record exists
            var existingAvailability = await _context.EmployeeAvailability
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId &&
                                        a.ServiceId == serviceId &&
                                        a.Date == date);

            if (isAvailable)
            {
                if (existingAvailability == null)
                {
                    // Create new availability record
                    var availability = new EmployeeAvailability
                    {
                        EmployeeId = employeeId,
                        ServiceId = serviceId,
                        Date = date,
                        IsBooked = false
                    };
                    _context.EmployeeAvailability.Add(availability);
                }
                else
                {
                    existingAvailability.IsBooked = false;
                }
            }
            else if (existingAvailability != null)
            {
                _context.EmployeeAvailability.Remove(existingAvailability);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
