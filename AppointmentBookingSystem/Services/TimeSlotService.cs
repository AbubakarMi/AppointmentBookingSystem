// Services/TimeSlotService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentBookingSystem.Models;

namespace AppointmentBookingSystem.Services
{
    public interface ITimeSlotService
    {
        bool IsAvailable(DateTime start, DateTime end);
        IEnumerable<Appointment> GetAllAppointments();
        bool CancelAppointment(int id);
        bool RescheduleAppointment(int id, Appointment updated);
    }

    public class TimeSlotService : ITimeSlotService
    {
        private readonly List<Appointment> _appointments = new();

        public bool IsAvailable(DateTime start, DateTime end)
        {
            return !_appointments.Any(a =>
                (start < a.EndTime && end > a.StartTime));
        }

        public IEnumerable<Appointment> GetAllAppointments() => _appointments;

        public bool CancelAppointment(int id)
        {
            var appt = _appointments.FirstOrDefault(a => a.Id == id);
            if (appt == null) return false;
            _appointments.Remove(appt);
            return true;
        }

        public bool RescheduleAppointment(int id, Appointment updated)
        {
            var appt = _appointments.FirstOrDefault(a => a.Id == id);
            if (appt == null) return false;

            appt.StartTime = updated.StartTime;
            appt.EndTime = updated.EndTime;
            appt.PatientName = updated.PatientName;
            appt.Notes = updated.Notes;

            return true;
        }
    }
}
