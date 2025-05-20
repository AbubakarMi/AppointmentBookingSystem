// Services/IAppointmentService.cs
public interface IAppointmentService
{
    Task<bool> BookAppointment(Appointment appointment);
}
