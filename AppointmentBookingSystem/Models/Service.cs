// Models/Service.cs
using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingSystem.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
