using AppointmentBookingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentBookingSystem.Services
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly List<Service> _services = new();

        public IEnumerable<Service> GetAll() => _services;

        public Service? GetById(int id) => _services.FirstOrDefault(s => s.Id == id);

        public void Add(Service service)
        {
            service.Id = _services.Count + 1;
            _services.Add(service);
        }

        public bool Update(int id, Service updated)
        {
            var service = _services.FirstOrDefault(s => s.Id == id);
            if (service == null) return false;

            service.Name = updated.Name;
            service.Description = updated.Description;
            return true;
        }

        public bool Delete(int id)
        {
            var service = _services.FirstOrDefault(s => s.Id == id);
            if (service == null) return false;

            _services.Remove(service);
            return true;
        }
    }
}