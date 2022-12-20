using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Filter;
using Hairdresser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Hairdresser.Core.Entities.Enums.Status;

namespace Hairdresser.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public HairdresserDbContext _ctx { get; set; }

        public AppointmentRepository(HairdresserDbContext ctx)
        {
            _ctx = ctx;
        }
        public Appointment Create(Appointment entity)
        {
            if (_ctx.Appointment.Any(o => o.AppointmentDateTime.CompareTo(entity.AppointmentDateTime) == 0 && o.EmployeeID == entity.EmployeeID))
            {
                throw new Exception("Denne tid er allerede optaget, vælg venligst en anden.");
            }
            var addedAppointment = _ctx.Appointment.Add(entity).Entity;
            _ctx.SaveChanges();
            return addedAppointment;
        }

        public Appointment Delete(Appointment entity)
        {
            var removedAppointment = _ctx.Appointment.Remove(entity).Entity;
            _ctx.SaveChanges();
            return removedAppointment;
        }

        public List<Appointment> Read()
        {
            return _ctx.Appointment.ToList();
        }

        public Appointment Update(Appointment entity)
        {
            var result = _ctx.Appointment.Attach(entity);
            _ctx.Entry(entity).Property(x => x.Status).IsModified = true;
            var updatedRows = _ctx.SaveChanges();
            return result.Entity;
        }

        public Appointment Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Appointment Get(DateTime time)
        {
            Appointment foundAppontment = null;
            foreach (Appointment a in _ctx.Appointment)
            {
                if (a.AppointmentDateTime.CompareTo(time) == 0)
                {
                    foundAppontment = a;
                    return foundAppontment;
                }
            }
            return foundAppontment;
        }

        public List<Appointment> GetActiveAppointmentsForUser(int userId)
        {
            var appointments =
                _ctx.Appointment
                .Where(x => x.CustomerID == userId)
                .Where(y => y.AppointmentDateTime >= DateTime.Now)
                .Where(z => z.Status == (int)Statuses.pending)
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Treatment)
                .ToList();

            return appointments;

        }

        public List<Appointment> GetOldAppointmentsForUser(int userId)
        {
            var appointments =
                _ctx.Appointment
                .Where(x => x.CustomerID == userId)
                .Where(y => y.AppointmentDateTime < DateTime.Now)
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.Treatment)
                .ToList();

            return appointments;
        }

        public FilterResponse<AppointmentDTO> GetAppointmentsForAdmin(AppointmentFilter filter)
        {
            var filteredResponse = new FilterResponse<AppointmentDTO>();

            IEnumerable<AppointmentDTO> filtering = _ctx.Appointment
                .Select(x => new AppointmentDTO()
                {
                    Customer = new UserBasicDTO()
                    {
                        Email = x.Customer.Email,
                        Firstname = x.Customer.FirstName,
                        Lastname = x.Customer.LastName,
                        UserID = x.Customer.UserID,
                    },
                    Employee = new UserBasicDTO()
                    {
                        Email = x.Employee.Email,
                        Firstname = x.Employee.FirstName,
                        Lastname = x.Employee.LastName,
                        UserID = x.Employee.UserID,
                    },
                    AppointmentDateTime = x.AppointmentDateTime,
                    Treatment = x.Treatment,
                    Status = x.Status,
                });

            filteredResponse.TotalCount = filtering.Count();

            // Search
            if (!string.IsNullOrEmpty(filter.Filter.SearchText))
            {
                switch (filter.Filter.SearchField)
                {
                    case "name":
                        filtering = filtering
                            .Where(x => (x.Customer.Firstname + " " + x.Customer.Lastname).ToLower().Contains(filter.Filter.SearchText.ToLower()));
                        break;
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(filter.Filter.OrderDirection) || !string.IsNullOrEmpty(filter.Filter.OrderProperty))
            {
                var dir = filter.Filter.OrderDirection;
                switch (filter.Filter.OrderProperty)
                {
                    case "customer":
                        filtering = "asc".Equals(filter.Filter.OrderDirection) ?
                            filtering.OrderBy(x => x.Customer.Firstname) :
                            filtering.OrderByDescending(x => x.Customer.Firstname);
                        break;
                    case "date":
                        filtering = "asc".Equals(filter.Filter.OrderDirection) ?
                            filtering.OrderBy(x => x.AppointmentDateTime) :
                            filtering.OrderByDescending(x => x.AppointmentDateTime);
                        break;
                    case "employee":
                        filtering = "asc".Equals(filter.Filter.OrderDirection) ?
                            filtering.OrderBy(x => x.Employee.Firstname) :
                            filtering.OrderByDescending(x => x.Employee.Firstname);
                        break;
                    case "service":
                        filtering = "asc".Equals(filter.Filter.OrderDirection) ?
                            filtering.OrderBy(x => x.Treatment.TreatmentName) :
                            filtering.OrderByDescending(x => x.Treatment.TreatmentName);
                        break;
                }
            }

            // Custom filters
            if (!filter.ShowOld)
            {
                filtering = filtering.Where(x => x.AppointmentDateTime >= DateTime.Now);
            }
            if (filter.FilterEmployeeId != 0)
            {
                filtering = filtering.Where(x => x.Employee.UserID == filter.FilterEmployeeId);
            }
            if (filter.ShowOld)
            {
                filtering = filtering.Where(x => x.AppointmentDateTime <= DateTime.Now);
            }
            if (filter.ShowCancelled)
            {
                filtering = filtering.Where(x => x.Status == 2);
            }

            filteredResponse.List = filtering.ToList();
            filteredResponse.ResultsFound = filtering.Count();

            return filteredResponse;
        }

        public List<Appointment> ReadAppointmentsByUser(User user)
        {
            throw new NotImplementedException();
        }

        public Appointment Get(int Id, DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
