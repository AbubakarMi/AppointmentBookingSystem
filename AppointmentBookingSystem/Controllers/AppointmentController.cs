// Controllers/AppointmentController.cs
using Microsoft.AspNetCore.Mvc;
using AppointmentBookingSystem.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppointmentController : ControllerBase
    {
        private static List<Appointment> _appointments = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(_appointments);

        [HttpPost]
        public IActionResult Book([FromBody] Appointment appointment)
        {
            appointment.Id = _appointments.Count + 1;
            _appointments.Add(appointment);
            return Ok("Appointment booked successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Reschedule(int id, [FromBody] Appointment updated)
        {
            var existing = _appointments.FirstOrDefault(a => a.Id == id);
            if (existing == null) return NotFound("Appointment not found");

            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.PatientName = updated.PatientName;
            existing.Notes = updated.Notes;

            return Ok("Appointment rescheduled");
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null) return NotFound("Appointment not found");

            _appointments.Remove(appointment);
            return Ok("Appointment cancelled");
        }
    }
}
