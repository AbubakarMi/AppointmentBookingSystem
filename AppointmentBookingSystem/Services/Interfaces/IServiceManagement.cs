using AppointmentBookingSystem.Models;
using System.Collections.Generic;

namespace AppointmentBookingSystem.Services
{
    public interface IServiceManagement
    {
        IEnumerable<Service> GetAll();
        Service? GetById(int id);
        void Add(Service service);
        bool Update(int id, Service updated);
        bool Delete(int id);
    }
}
