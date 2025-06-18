using Microsoft.EntityFrameworkCore;
using AppointmentBookingSystem.Models;

namespace AppointmentBookingSystem
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
    }
}