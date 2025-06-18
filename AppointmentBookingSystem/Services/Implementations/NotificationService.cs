using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AppointmentBookingSystem.Models;
using AppointmentBookingSystem.Services;


public class NotificationService : INotificationService
{
    public async Task SendBookingConfirmationAsync(Appointment appointment)
    {
        var smtpClient = new SmtpClient("smtp.yourmailserver.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
            EnableSsl = true,
        };

        var mail = new MailMessage
        {
            From = new MailAddress("your-email@example.com"),
            Subject = "Booking Confirmation",
            Body = $"Hello {appointment.PatientName}, your appointment is booked from {appointment.StartTime} to {appointment.EndTime}.",
            IsBodyHtml = false,
        };

        // Replace with actual email
        mail.To.Add("recipient@example.com");

        await smtpClient.SendMailAsync(mail);
    }
}
