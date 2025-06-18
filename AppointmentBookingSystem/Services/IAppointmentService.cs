using System.Threading.Tasks;
using AppointmentBookingSystem.Models;

namespace AppointmentBookingSystem.Services
{
    public interface IAppointmentService
    {
        Task<bool> BookAppointment(Appointment appointment);
    }
}
