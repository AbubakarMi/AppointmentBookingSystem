using Microsoft.AspNetCore.Mvc;
using AppointmentBookingSystem.Models;
using AppointmentBookingSystem.Services;

namespace AppointmentBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceManagement _serviceManager;

        public ServiceController(IServiceManagement serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_serviceManager.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var service = _serviceManager.GetById(id);
            return service is null ? NotFound() : Ok(service);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Service service)
        {
            _serviceManager.Add(service);
            return Ok("Service added");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Service service)
        {
            return _serviceManager.Update(id, service)
                ? Ok("Service updated")
                : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return _serviceManager.Delete(id)
                ? Ok("Service deleted")
                : NotFound();
        }
    }
}