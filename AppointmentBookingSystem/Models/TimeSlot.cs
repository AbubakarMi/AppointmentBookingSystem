namespace AppointmentBookingSystem.Models;

public class TimeSlot
{
    public int Id { get; set; }
    public bool IsBooked { get; set; }
    public List<Appointment>? Appointments { get; set; }
}