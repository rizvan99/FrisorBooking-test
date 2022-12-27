using Hairdresser.Core.Application.Interfaces;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Exceptions.Service.General;
using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using static Hairdresser.Core.Entities.Enums.Status;

namespace Hairdresser.Core.Application.Implementation
{
    public class AppointmentService : ICRUDService<Appointment>, IValidator<Appointment>
    {
        public IAppointmentRepository _repository;
        private static int OPEN_HOURS = 9;
        private static int CLOSING_HOURS = 18;
        /// <summary>
        /// In hours
        /// </summary>
        private static double TIME_PER_CUSTOMER = 0.5;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository ?? throw new ArgumentException("Repository is missing");
        }
        public void ValidateEntity(Appointment entity)
        {
            if (entity.AppointmentDateTime == null)
            {
                throw new EntityDataMissingException("Vælg venligt tid og dato du vil booke");
            }
            if (entity.AppointmentDateTime.CompareTo(DateTime.Now.AddYears(1)) > 0)
            {
                throw new EntityDataMissingException("Du kan højest booke din tid et år frem");
            }
            if (entity.TreatmentID == 0)
            {
                throw new EntityDataMissingException("Vælg venligst hvilket arbejde der skal udføres");
            }
            EntityExists(entity);
        }
        public bool EntityExists(Appointment entity)
        {
            if (entity == null)
            {
                throw new EntityDataMissingException("Tiden blev ikke booket, prøv igen senere");
            };
            return true;
        }
        public Appointment Create(Appointment entity)
        {
            ValidateEntity(entity);
            try
            {
                return _repository.Create(entity);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public Appointment Delete(Appointment entity)
        {
            if (EntityExists(entity))
            {
                try
                {
                    var result = _repository.Delete(entity);
                    if (result == null)
                    {
                        throw new ReplyIsNullOrEmptyException("Denne aftale eksisterer ikke");
                    }
                    return result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new EntityDataMissingException("Brugeren blev ikke fundet");
            }
        }

        public List<Appointment> Read()
        {
            try
            {
                var result = _repository.Read();
                if (result == null || result.Count == 0)
                {
                    throw new ListIsNullOrEmptyException("Der blev ikke fundet nogen tider");
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// Updates a given appointment entity in the database
        /// </summary>
        /// <param name="entity">the appointment you would like to update</param>
        /// <returns>the updated appoinment</returns>
        public Appointment Update(Appointment entity)
        {
            ValidateEntity(entity);
            try
            {
                if (EntityExists(entity))
                {
                    var result = _repository.Update(entity);
                    return result;
                }
                throw new EntityDataMissingException("Der mangler data før vi kan opdatere Aftalen");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Gets all future appointments
        /// </summary>
        /// <returns>A list of all upcomming appointments in the database</returns>
        public List<Appointment> GetUpcommingAppointments()
        {
            try
            {
                List<Appointment> upcommingAppointments = new List<Appointment>();
                foreach (Appointment a in _repository.Read())
                {
                    if (a.AppointmentDateTime.CompareTo(DateTime.Now) > 0 || a.AppointmentDateTime.CompareTo(DateTime.Now) == 0)
                    {
                        upcommingAppointments.Add(a);
                    }
                }
                if (upcommingAppointments == null || upcommingAppointments.Count == 0)
                {
                    throw new ListIsNullOrEmptyException("Der er ingen kommende tider");
                }
                return upcommingAppointments;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Returns all apointments on an after a given date
        /// </summary>
        /// <param name="date">The date you want to filter after</param>
        /// <returns>A list of appointments after and including the input date</returns>
        public List<Appointment> GetAppointmentsAfterDateTime(DateTime date)
        {
            try
            {
                List<Appointment> upcommingAppointments = new List<Appointment>();
                foreach (Appointment a in _repository.Read())
                {
                    if (a.AppointmentDateTime.CompareTo(date) > 0 || a.AppointmentDateTime.CompareTo(date) == 0)
                    {
                        upcommingAppointments.Add(a);
                    }
                }
                if (upcommingAppointments == null || upcommingAppointments.Count == 0)
                {
                    throw new ListIsNullOrEmptyException("Der er ingen kommende tider efter denne dato");
                }
                return upcommingAppointments;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Get appointments on a given date
        /// </summary>
        /// <param name="date">The date you want to find appointments for</param>
        /// <returns>A list of all appointments on a given date</returns>
        public List<Appointment> GetAppointmentsOnDateTime(DateTime date)
        {
            try
            {
                List<Appointment> upcommingAppointments = new List<Appointment>();
                foreach (Appointment a in _repository.Read())
                {
                    if (a.AppointmentDateTime.Date.CompareTo(date.Date) == 0)
                    {
                        upcommingAppointments.Add(a);
                    }
                }
                if (upcommingAppointments == null || upcommingAppointments.Count == 0)
                {
                    throw new ListIsNullOrEmptyException("Der er ingen tider på denne dato");
                }
                return upcommingAppointments;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Appointment CompleteAssignemnt(Appointment entity)
        {
            ValidateEntity(entity);
            if (EntityExists(entity))
            {
                entity.Status = (int)Statuses.completed;
                try
                {
                    return _repository.Update(entity);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new ArgumentException("Der gik noget galt");
            }
        }
        public Appointment CancelAssignemnt(Appointment entity)
        {
            ValidateEntity(entity);
            if (EntityExists(entity))
            {
                entity.Status = (int)Statuses.cancelled;
                return _repository.Update(entity);
            }
            else
            {
                throw new ArgumentException("Der gik noget galt");
            }
        }

        public Appointment Get(int time, DateTime date)
        {
            return _repository.Get(time, date);
        }
        /// <summary>
        /// Checks if a time is evalable on a given date.
        /// </summary>
        /// <param name="date">The date you want to check the time for</param>
        /// <returns>false if the DateTime is unavalable and true if it is avalable</returns>
        public Boolean CheckIfTimeIsAvalable(DateTime date)
        {
            var appointmentsOnDate = new List<DateTime>();
            //Creates a list of all appointments on a given date
            foreach (Appointment a in _repository.Read())
            {
                if (a.AppointmentDateTime.Date.CompareTo(date.Date) == 0)
                {
                    appointmentsOnDate.Add(a.AppointmentDateTime);
                }
            }
            //Checks if an appointment datetime is equal to given datetime
            foreach (DateTime dt in appointmentsOnDate)
            {
                if (date.CompareTo(dt) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Checks if a given date has any avalable times
        /// </summary>
        /// <param name="date">the date you want to check</param>
        /// <returns>false if the date is fully booked and true if there are times avalable</returns>
        public Boolean CheckIfDateHasAvalableTimes(DateTime date)
        {
            var appointmentsOnDate = new List<DateTime>();
            foreach (Appointment a in _repository.Read())
            {
                if (a.AppointmentDateTime.Date.CompareTo(date.Date) == 0)
                {
                    appointmentsOnDate.Add(a.AppointmentDateTime);
                }
            }
            if (appointmentsOnDate.Count >= (CLOSING_HOURS - OPEN_HOURS) / TIME_PER_CUSTOMER)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// generates a list of appointment times sepperated by hour for the frontend.
        /// </summary>
        /// <param name="date">the date you want hours for</param>
        /// <returns>A list of datetimes with all the hours in the day</returns>
        public List<DateTime> GenerateTimeSchedule(DateTime date)
        {
            date = date.ToLocalTime();
            var avalableTimeSlots = new List<DateTime>();
            var initialTime = new DateTime(date.Year, date.Month, date.Day, OPEN_HOURS, 0, 0);
            avalableTimeSlots.Add(initialTime);
            while (initialTime.Hour < CLOSING_HOURS - 1)
            {
                initialTime = initialTime.AddMinutes(30);
                if (_repository.Get(initialTime) == null)
                {
                    avalableTimeSlots.Add(initialTime);
                }
            }
            return avalableTimeSlots;
        }
        /// <summary>
        /// Generates a list with dates from now until seven days later
        /// </summary>
        /// <returns>a list of 7 DateTimes</returns>
        public List<CalendarDateModel> GenerateDateSchedule()
        {
            var avalableTimeSlots = new List<CalendarDateModel>();
            var initialTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, OPEN_HOURS, 0, 0);
            if (initialTime.DayOfWeek != DayOfWeek.Monday && initialTime.DayOfWeek != DayOfWeek.Sunday)
            {
                avalableTimeSlots.Add(new CalendarDateModel()
                {
                    AppointmentTime = initialTime,
                    Taken = CheckIfDateHasAvalableTimes(initialTime)
                });
            }

            while (avalableTimeSlots.Count < 7)
            {
                initialTime = initialTime.AddDays(1);
                if (initialTime.DayOfWeek != DayOfWeek.Monday && initialTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    avalableTimeSlots.Add(new CalendarDateModel()
                    {
                        AppointmentTime = initialTime,
                        Taken = CheckIfDateHasAvalableTimes(initialTime)
                    });
                }

            }

            return avalableTimeSlots;
        }

        public List<Appointment> GetActiveAppointmentsForUser(int userId)
        {
            return _repository.GetActiveAppointmentsForUser(userId);
        }

        public List<Appointment> GetOldAppointmentsForUser(int userId)
        {
            return _repository.GetOldAppointmentsForUser(userId);
        }

        public FilterResponse<AppointmentDTO> GetWithFilter(AppointmentFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Filter.SearchText) && string.IsNullOrEmpty(filter.Filter.SearchField))
            {
                filter.Filter.SearchField = "name";
            }

            return _repository.GetAppointmentsForAdmin(filter);
        }

        public Appointment Get(DateTime time)
        {
            throw new NotImplementedException();
        }

        public Appointment Get(int id)
        {
            throw new NotImplementedException();
        }

        public FilterResponse<Appointment> GetWithFilter(Filter.Filter filter)
        {
            throw new NotImplementedException();
        }
    }
}
