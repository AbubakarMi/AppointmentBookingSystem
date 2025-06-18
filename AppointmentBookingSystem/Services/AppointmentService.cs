using AppointmentBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AppointmentBookingSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _db;

        public AppointmentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> BookAppointment(Appointment appointment)
        {
            _db.Appointments.Add(appointment);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
