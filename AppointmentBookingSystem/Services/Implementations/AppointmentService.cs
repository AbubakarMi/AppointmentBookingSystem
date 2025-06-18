using AppointmentBookingSystem.Models;
using AppointmentBookingSystem.Services;
using AppointmentBookingSystem;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notificationService;

    public AppointmentService(AppDbContext db, INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }

    public async Task<bool> BookAppointment(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        var success = await _db.SaveChangesAsync() > 0;

        if (success)
        {
            await _notificationService.SendBookingConfirmationAsync(appointment);
        }

        return success;
    }
}