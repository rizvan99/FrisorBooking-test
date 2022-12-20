using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Domain.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// searches the database for a user with a specific appointment based on the employee executing the service
        /// </summary>
        /// <param name="date">the date and time of the appointment</param>
        /// <param name="employee">the employee with the task</param>
        /// <returns>a single customer who matches the parameters</returns>
        public User GetByAppointment(DateTime date, User employee);
        /// <summary>
        /// searches a database for users who have an appointment at a given time
        /// </summary>
        /// <param name="date">the date and time you want to see customers for</param>
        /// <returns>all users who have an appointment on a given date and time</returns>
        public List<User> ReadByAppointment(DateTime date);
        /// <summary>
        /// This method searches the database for all customers
        /// </summary>
        /// <returns>A list of all customers in the system</returns>
        public FilterResponse<UserBasicDTO> GetCustomers(Filter.Filter filter);
        /// <summary>
        /// This method searches the database for all admin user
        /// </summary>
        /// <returns>A list of all admins in the system</returns>
        public List<User> GetAdmins();
        /// <summary>
        /// Adds a user entity to the database
        /// </summary>
        /// <param name="entity">the user you want to add</param>
        /// <returns>the newly created user</returns>
        public User Create(User entity);
        /// <summary>
        /// Finds every user on the database
        /// </summary>
        /// <returns>A list of all users on the database</returns>
        public List<User> Read();
        /// <summary>
        /// This method is used to update all the users information including admin status.
        /// </summary>
        /// <param name="entity">the user you want to update</param>
        /// <returns>the updated entity</returns>
        public User Update(User entity);
        /// <summary>
        /// This method is used to update only a users contact and name information.
        /// </summary>
        /// <param name="entity">The entity you want to update</param>
        /// <returns>The altered user</returns>
        public UserDTO UpdateBasicUserInfo(User entity);
        /// <summary>
        /// Removes a user from the database
        /// </summary>
        /// <param name="entity">the user you want to delete</param>
        /// <returns>the user who was removed</returns>
        public User Delete(User entity);
        /// <summary>
        /// Searches the database for a user with a specific id on the database
        /// </summary>
        /// <param name="Id">the id you want to find</param>
        /// <returns>the user found</returns>
        public User Get(int Id);
        public void ChangePassword(User entity);
    }
}
