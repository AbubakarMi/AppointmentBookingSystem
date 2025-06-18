using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingSystem.Models
{
    public class ViewModels
    {
    }

    public class AppointmentViewModel
    {
        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [DateGreaterThan("StartTime", ErrorMessage = "End time must be after start time")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Patient name is required")]
        [MinLength(2, ErrorMessage = "Patient name must be at least 2 characters")]
        public string PatientName { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");

            var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue != null && comparisonValue != null && currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }

    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string Role { get; set; } = "User"; // Default role
    }
}
