// Models/Appointment.cs
public class Appointment
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public string PatientName { get; set; }
    public string Notes { get; set; }
}