// DbContext/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using AppointmentBookingSystem.Models;

namespace AppointmentBookingSystem
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
