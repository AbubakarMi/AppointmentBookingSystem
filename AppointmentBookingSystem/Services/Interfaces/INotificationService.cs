using AppointmentBookingSystem.Models;

namespace AppointmentBookingSystem.Services
{
    public interface INotificationService
    {
        Task SendBookingConfirmationAsync(Appointment appointment);
    }
}