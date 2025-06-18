using System;

namespace AppointmentBookingSystem.Services
{
    public class NotificationService : INotificationService
    {
        public void SendBookingConfirmation(string to, string message)
        {
            // Simulate sending email/SMS
            Console.WriteLine($"Notification sent to {to}: {message}");
        }
    }
}