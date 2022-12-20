using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Filter;
using Hairdresser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hairdresser.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public HairdresserDbContext _ctx { get; set; }
        public UserRepository(HairdresserDbContext ctx)
        {
            _ctx = ctx;
        }
        /// <summary>
        /// Adds a user entity to the database
        /// </summary>
        /// <param name="entity">the user you want to add</param>
        /// <returns>the newly created user</returns>
        public User Create(User entity)
        {
            try
            {
                var addedUser = _ctx.Users.Add(entity).Entity;
                _ctx.SaveChanges();
                return addedUser;
            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Der gik noget galt under oprettelsen af denne bruger. Kontakt IT: " + e.Message);
            }
        }
        /// <summary>
        /// Removes a user from the database
        /// </summary>
        /// <param name="entity">the user you want to delete</param>
        /// <returns>the user who was removed</returns>
        public User Delete(User entity)
        {
            try
            {
                var deletedUser = _ctx.Remove(entity).Entity;
                _ctx.SaveChanges();
                return deletedUser;
            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Der gik noget galt i forsøget på at slette denne bruger. Kontakt IT: " + e.Message);
            }
        }
        /// <summary>
        /// Searches the database for a user with a specific id on the database
        /// </summary>
        /// <param name="Id">the id you want to find</param>
        /// <returns>the user found</returns>
        public User Get(int Id)
        {
            try
            {
                var foundUser = _ctx.Users.FirstOrDefault(x => x.UserID == Id);
                if (foundUser == null)
                {
                    throw new ArgumentException("Brugeren blev ikke fundet");
                }
                return foundUser;
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException("Der gik noget galt i databasen. Kontakt IT: " + e.Message);
            }
        }
        /// <summary>
        /// Finds every user on the database
        /// </summary>
        /// <returns>A list of all users on the database</returns>
        public List<User> Read()
        {
            var result = _ctx.Users.ToList();
            if (result.Count <= 0)
            {
                throw new ArgumentException("Der er ingen burgere at vise");
            }
            return _ctx.Users.ToList();
        }
        /// <summary>
        /// This method is used to update all the users information including admin status.
        /// </summary>
        /// <param name="entity">the user you want to update</param>
        /// <returns>the updated entity</returns>
        public User Update(User entity)
        {
            try
            {
                _ctx.Users.Attach(entity);
                _ctx.Entry(entity).Property(x => x.FirstName).IsModified = true;
                _ctx.Entry(entity).Property(x => x.LastName).IsModified = true;
                _ctx.Entry(entity).Property(x => x.Email).IsModified = true;
                _ctx.Entry(entity).Property(x => x.PhoneNumber).IsModified = true;
                _ctx.Entry(entity).Property(x => x.IsAdmin).IsModified = true;

                _ctx.SaveChanges();
                return entity;
            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Noget gik galt i databsen under opdateringen af denne brugers data. Kontakt IT: " + e.Message);
            }

        }
        /// <summary>
        /// This method searches the database for all customers
        /// </summary>
        /// <returns>A list of all customers in the system</returns>
        public FilterResponse<UserBasicDTO> GetCustomers(Filter filter)
        {
            var filteredResponse = new FilterResponse<UserBasicDTO>();

            IEnumerable<UserBasicDTO> filtering = _ctx.Users
                .Where(x => x.IsAdmin == false)
                .Select(x => new UserBasicDTO()
                {
                    Email = x.Email,
                    Firstname = x.FirstName,
                    Lastname = x.LastName,
                    UserID = x.UserID,
                    PhoneNumber = x.PhoneNumber,
                });

            filteredResponse.TotalCount = filtering.Count();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                switch (filter.SearchField)
                {
                    case "name":
                        filtering = filtering
                            .Where(x => (x.Firstname + " " + x.Lastname).ToLower().Contains(filter.SearchText.ToLower()) ||
                            (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(filter.SearchText.ToLower())));
                        break;
                }
            }

            filteredResponse.List = filtering.ToList();
            filteredResponse.ResultsFound = filtering.Count();

            return filteredResponse;
        }

        /// <summary>
        /// This method searches the database for all admin user
        /// </summary>
        /// <returns>A list of all admins in the system</returns>
        public List<User> GetAdmins()
        {
            var result = _ctx.Users.
                Where(x => x.IsAdmin == true).ToList();
            if (result.Count == 0 || result == null)
            {
                throw new ArgumentException("Der blev ikke fundet nogen admin i databasen");
            }
            return result;
        }
        /// <summary>
        /// searches a database for users who have an appointment at a given time
        /// </summary>
        /// <param name="date">the date and time you want to see customers for</param>
        /// <returns>all users who have an appointment on a given date and time</returns>
        public List<User> ReadByAppointment(DateTime date)
        {
            var result = (from user in _ctx.Users
                          join appointment in _ctx.Appointment
                          on user.UserID equals appointment.CustomerID
                          where appointment.AppointmentDateTime.CompareTo(date) == 0
                          select user).ToList();
            if (result.Count <= 0)
            {
                throw new ArgumentException("Der blev ikke fundet nogle kunder med denne tid i databasen");
            }
            return result;
        }
        /// <summary>
        /// searches the database for a user with a specific appointment based on the employee executing the service
        /// </summary>
        /// <param name="date">the date and time of the appointment</param>
        /// <param name="employee">the employee with the task</param>
        /// <returns>a single customer who matches the parameters</returns>
        public User GetByAppointment(DateTime date, User employee)
        {
            var result = (from user in _ctx.Users
                          join appointment in _ctx.Appointment
                          on user.UserID equals appointment.CustomerID
                          where appointment.AppointmentDateTime.CompareTo(date) == 0 && employee.UserID == appointment.EmployeeID
                          select user).SingleOrDefault();
            if (result == null)
            {
                throw new ArgumentException("Der er ikke fundet nogen kunde med denne tid i databasen");
            }
            return result;
        }
        /// <summary>
        /// This method is used to update only a users contact and name information.
        /// </summary>
        /// <param name="entity">The entity you want to update</param>
        /// <returns>The altered user</returns>
        public UserDTO UpdateBasicUserInfo(User entity)
        {
            try
            {
                var result = _ctx.Users.Attach(entity);
                _ctx.Entry(entity).Property(x => x.FirstName).IsModified = true;
                _ctx.Entry(entity).Property(x => x.LastName).IsModified = true;
                _ctx.Entry(entity).Property(x => x.Email).IsModified = true;
                var updateRows = _ctx.SaveChanges();
                if (updateRows == 1)
                {
                    var user = Get(entity.UserID);
                    return new UserDTO()
                    {
                        Email = user.Email,
                        Firstname = user.FirstName,
                        Lastname = user.LastName,
                        UserID = user.UserID,
                        IsAdmin = user.IsAdmin,
                        PhoneNumber = user.PhoneNumber,
                    };
                }
                else if (updateRows > 1)
                {
                    throw new ArgumentException("Brugeren gav flere hits på databasen. Kontakt IT");
                }
                else
                {
                    throw new ArgumentException("Brugeren gav ingen hits på databasen. Kontakt IT");
                }
            }
            catch (DbUpdateException e)
            {
                throw new ArgumentException("Noget gik galt med databasen. Kontakt IT: " + e.Message);
            }
        }

        public void ChangePassword(User entity)
        {
            var result = _ctx.Users.Attach(entity);
            _ctx.Entry(entity).Property(x => x.PasswordHash).IsModified = true;
            _ctx.Entry(entity).Property(x => x.PasswordSalt).IsModified = true;
            _ctx.SaveChanges();

        }
    }
}
