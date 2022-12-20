using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Calls repositories create method to add a user to the database
        /// </summary>
        /// <param name="entity">the user you want to add</param>
        /// <returns>the added user</returns>
        public User Create(User entity);
        /// <summary>
        /// Calls the read method of the repository, to get all users in the database
        /// </summary>
        /// <returns>All users from the database</returns>
        public List<User> Read();
        /// <summary>
        /// Calls the repositories update method to update a given entry
        /// </summary>
        /// <param name="entity">the user you want to update</param>
        /// <returns>the updated user</returns>
        public User Update(User entity);
        /// <summary>
        /// Calls the repositories delete method to remove a given entry
        /// </summary>
        /// <param name="entity">the user you want to remove</param>
        /// <returns>the removed user</returns>
        public User Delete(User entity);
        /// <summary>
        /// Calls the repositories get method to retrieve a specific user from an id
        /// </summary>
        /// <param name="id">the id of the user you are trying to retrieve</param>
        /// <returns>the user mathching the criteria</returns>
        public User Get(int id);
        /// <summary>
        /// Calls the repositories get admins method to retrieve all admins in the database
        /// </summary>
        /// <returns>a list of all admins found in the database</returns>
        public List<UserBasicDTO> GetAdmins();
        /// <summary>
        /// Calls the repositories get customers method to retrieve all customers in the database
        /// </summary>
        /// <returns>a list of all customers found in the database</returns>
        public FilterResponse<UserBasicDTO> GetCustomers(Filter.Filter filter);
        /// <summary>
        /// Calls the repositories update method for
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO UpdateBasicUserInfo(UserDTO entity);
        public void ChangePassword(int id, byte[] passHash, byte[] passSalt);
    }
}
