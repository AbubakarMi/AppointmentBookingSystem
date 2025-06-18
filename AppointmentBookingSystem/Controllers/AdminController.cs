// Controllers/AdminController.cs
using Microsoft.AspNetCore.Mvc;
using AppointmentBookingSystem.Models;
using AppointmentBookingSystem.Services;
using System;
using System.Linq;

namespace AppointmentBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult AppointmentsToday()
        {
            var today = DateTime.Today;
            var count = _db.Appointments.Count(a => a.StartTime.Date == today);
            return Ok(new { Date = today, Count = count });
        }

        [HttpGet]
        public IActionResult PeakHours()
        {
            var hours = _db.Appointments
                .GroupBy(a => a.StartTime.Hour)
                .Select(g => new { Hour = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(3)
                .ToList();

            return Ok(hours);
        }
    }
}
