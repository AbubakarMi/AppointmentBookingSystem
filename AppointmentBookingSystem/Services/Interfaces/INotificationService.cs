namespace AppointmentBookingSystem.Services
{
    public interface INotificationService
    {
        void SendBookingConfirmation(string to, string message);
    }
}