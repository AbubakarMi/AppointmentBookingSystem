// Models/Appointment.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}