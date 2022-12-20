using Hairdresser.Core.Entities;
using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        public Appointment Create(Appointment entity);
        public List<Appointment> Read();
        public Appointment Update(Appointment entity);
        public Appointment Delete(Appointment entity);
        public Appointment Get(int Id, DateTime dateTime);
        public List<Appointment> GetActiveAppointmentsForUser(int userId);
        public List<Appointment> GetOldAppointmentsForUser(int userId);
        public Appointment Get(DateTime dt);
        public List<Appointment> ReadAppointmentsByUser(User user);
        public Appointment Get(int v);
        public FilterResponse<AppointmentDTO> GetAppointmentsForAdmin(AppointmentFilter filter);
    }
}
