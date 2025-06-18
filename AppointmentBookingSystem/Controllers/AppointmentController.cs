
// Controllers/AppointmentController.cs
using Microsoft.AspNetCore.Mvc;
using AppointmentBookingSystem.Models;
using AppointmentBookingSystem.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ITimeSlotService _timeSlotService;

        public AppointmentController(IAppointmentService appointmentService, ITimeSlotService timeSlotService)
        {
            _appointmentService = appointmentService;
            _timeSlotService = timeSlotService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_timeSlotService.GetAllAppointments());

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] Appointment appointment)
        {
            if (!_timeSlotService.IsAvailable(appointment.StartTime, appointment.EndTime))
                return BadRequest("Time slot is already booked");

            var result = await _appointmentService.BookAppointment(appointment);
            return result ? Ok("Appointment booked successfully") : StatusCode(500, "Booking failed");
        }

        [HttpPut("{id}")]
        public IActionResult Reschedule(int id, [FromBody] Appointment updated)
        {
            var result = _timeSlotService.RescheduleAppointment(id, updated);
            return result ? Ok("Appointment rescheduled") : NotFound("Appointment not found");
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var result = _timeSlotService.CancelAppointment(id);
            return result ? Ok("Appointment cancelled") : NotFound("Appointment not found");
        }
    }
}