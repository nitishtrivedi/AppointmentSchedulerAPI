using AppScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace AppScheduler.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
    }
}
