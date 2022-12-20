using Hairdresser.Core.Application.Interfaces;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Entities.DTO;
using Hairdresser.Core.Exceptions.Service.General;
using Hairdresser.Core.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Application.Implementation
{
    public class UserService : IValidator<User>, IUserService
    {
        public readonly IUserRepository _userRepo;

        public UserService(IUserRepository repository)
        {
            _userRepo = repository ?? throw new ArgumentException("Repository is missing");
        }

        public void ValidateEntity(User entity)
        {
            if (string.IsNullOrEmpty(entity.PhoneNumber))
            {
                throw new EntityDataMissingException("Angiv venligst et telefonnummer");
            }
            if (entity.PasswordHash == null || entity.PasswordHash.Length <= 0)
            {
                throw new EntityDataMissingException("Angiv veligst et Kodeord");
            }
            if (entity == null)
            {
                throw new EntityIsNullOrEmptyException("Brugeren blev ikke lavet");
            }

        }
        public Boolean EntityExists(User entity)
        {
            try
            {
                return _userRepo.Get(entity.UserID) != null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public User Create(User entity)
        {
            ValidateEntity(entity);
            try
            {
                if (string.IsNullOrEmpty(entity.FirstName))
                {
                    throw new EntityIsNullOrEmptyException("Ugyldigt fornavn");
                }
                if (string.IsNullOrEmpty(entity.LastName))
                {
                    throw new EntityIsNullOrEmptyException("Ugyldigt efternavn");
                }
                if (string.IsNullOrEmpty(entity.Email))
                {
                    throw new EntityIsNullOrEmptyException("Ugyldigt fornavn");
                }
                return _userRepo.Create(entity);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public User Delete(User entity)
        {
            if (EntityExists(entity))
            {
                try
                {
                    return _userRepo.Delete(entity);
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

        public User Get(int Id)
        {
            try
            {
                var result = _userRepo.Get(Id);
                if (result == null)
                {
                    throw new EntityIsNullOrEmptyException("Brugeren blev ikke fundet");
                }
                return result;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public List<User> Read()
        {
            try
            {
                var result = _userRepo.Read();
                if (result == null || result.Count == 0)
                {
                    throw new ListIsNullOrEmptyException("Der blev ikke fundet nogle brugere");
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public User Update(User entity)
        {
            ValidateEntity(entity);
            try
            {
                if (!EntityExists(entity))
                {
                    throw new EntityDataMissingException("Brugeren, der skulle opdateres, blev ikke fundet");
                }
                return _userRepo.Update(entity);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public List<UserBasicDTO> GetAdmins()
        {
            var response = _userRepo.GetAdmins();
            try
            {
                if (response.Count != 0 || response != null)
                {
                    var result = new List<UserBasicDTO>();
                    foreach (User u in response)
                    {
                        result.Add(new UserBasicDTO()
                        {
                            UserID = u.UserID,
                            Email = u.Email,
                            Firstname = u.FirstName,
                            Lastname = u.LastName
                        });
                    }
                    return result;
                }
                else
                {
                    throw new ListIsNullOrEmptyException("Der er ingen adminstratore at vise");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public FilterResponse<UserBasicDTO> GetCustomers(Filter.Filter filter)
        {
            try
            {
                if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
                {
                    filter.SearchField = "name";
                }
                return _userRepo.GetCustomers(filter);

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public User getCustomerByAppointment(DateTime date, User user)
        {
            try
            {
                var result = _userRepo.GetByAppointment(date, user);
                if (result == null)
                {
                    throw new ReplyIsNullOrEmptyException("Databasen gav intet resultat med denne bruger");
                }
                return _userRepo.GetByAppointment(date, user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<User> ReadCustomersByAppointment(DateTime date)
        {
            try
            {
                var result = _userRepo.ReadByAppointment(date);
                if (result == null)
                {
                    throw new ListIsNullOrEmptyException("Der er ingen kunder at vise");
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UserDTO UpdateBasicUserInfo(UserDTO entity)
        {
            try
            {
                var result = _userRepo.UpdateBasicUserInfo(new User()
                {
                    Email = entity.Email,
                    FirstName = entity.Firstname,
                    LastName = entity.Lastname,
                    UserID = entity.UserID,
                });
                if (result == null)
                {
                    throw new ArgumentException("Opdateringen returnerede ikke noget svar.");
                }

                var updatedUser = new UserDTO()
                {
                    UserID = result.UserID,
                    Email = result.Email,
                    Firstname = result.Firstname,
                    Lastname = result.Lastname,
                    PhoneNumber = result.PhoneNumber,
                    IsAdmin = result.IsAdmin
                };
                return updatedUser;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public void ChangePassword(int id, byte[] passHash, byte[] passSalt)
        {
            var user = _userRepo.Get(id);

            if (user == null)
            {
                throw new ArgumentException("Brugeren blev ikke fundet, opdater siden og prøv igen");
            }

            if (passHash == null || passSalt == null)
            {
                throw new ArgumentException("Adgangskoden blev ikke opdateret på grund af en fejl. Opdater siden og prøv igen");
            }

            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;

            _userRepo.ChangePassword(user);
        }

        List<UserBasicDTO> IUserService.GetAdmins()
        {
            throw new NotImplementedException();
        }
    }
}
