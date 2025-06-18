// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using AppointmentBookingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private static List<User> Users = new();

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            if (Users.Any(u => u.Username == user.Username))
                return BadRequest("Username already exists");

            Users.Add(user);
            return Ok("Registration successful");
        }

        [HttpPost]
        public IActionResult Login([FromBody] User login)
        {
            var user = Users.FirstOrDefault(u => u.Username == login.Username && u.PasswordHash == login.PasswordHash);
            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok("Login successful");
        }
    }
}
