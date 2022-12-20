using FluentAssertions;
using Hairdresser.Core.Application.Implementation;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities.Auth;
using Hairdresser.Core.Exceptions.Service.General;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Hairdresser.XUnitTests
{
    public class UserServiceXUnitTests
    {
        private List<User> _users = null;
        private readonly Mock<IUserRepository> _userRep;
        public UserServiceXUnitTests()
        {
            _userRep = new Mock<IUserRepository>();
            _userRep.Setup(repo => repo.Read()).Returns(() => _users);
        }
        #region crudTests
        [Fact]
        public void CreateUserWithValidInputTest()
        {
            //Arrange
            var user = new User()
            {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11 11 11 11",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
            };

            var service = new UserService(_userRep.Object);

            _users = new List<User>();

            //Act
            var result = service.Create(user);
            _users.Add(user);

            //Assert
            _userRep.Setup(repo => repo.Create(user)).Returns(result);
            _userRep.Verify(repo => repo.Create(user), Times.Once);
            _users.Should().Contain(user);
        }

        [Fact]
        public void CreateUserWithInvalidPhoneNumberTest()
        {
            //Arrange
            var user = new User()
            {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
            };

            var service = new UserService(_userRep.Object);
            _users = new List<User>();

            //Act & Assert
            Assert.Throws<EntityDataMissingException>(() => service.Create(user));
            _userRep.Verify(repo => repo.Create(user), Times.Never);
        }
        [Fact]
        public void CreateUserWithNullInputTest()
        {
            //Arrange
            User user = null;

            var service = new UserService(_userRep.Object);
            _users = new List<User>();

            //Act & Assert
            Assert.Throws<NullReferenceException>(() => service.Create(user));
            _userRep.Verify(repo => repo.Create(user), Times.Never);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllUsersTest(int listCount)
        {
            // ARRANGE
            var _users = new List<User>()
            {
                new User()
                {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1 11 11 11",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
                },
                new User()
                {
                Email = "test@tests2.com",
                IsAdmin = true,
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "12 12 12 12",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
                }
        };

            var service = new UserService(_userRep.Object);

            _userRep.Setup(repo => repo.Read()).Returns(() => _users.GetRange(0, listCount));

            // ACT
            var tasksFound = service.Read();

            // ASSERT
            Assert.Equal(_users.GetRange(0, listCount), tasksFound);
            _userRep.Verify(repo => repo.Read(), Times.Once);
        }
        [Fact]
        public void GetAllWithNoUsersTest()
        {
            // ARRANGE
            var _users = new List<User>();
            var service = new UserService(_userRep.Object);

            // ACT & ASSERT
            Assert.Throws<ListIsNullOrEmptyException>(() => service.Read());
            _userRep.Verify(repo => repo.Read(), Times.Once);
        }
        [Fact]
        public void UpdateUserWithValidInputTest()
        {
            // ARRANGE
            var user = new User()
            {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11 11 11 11",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
            };

            var service = new UserService(_userRep.Object);

            _userRep.Setup(repo => repo.Get(It.Is<int>(z => z == user.UserID))).Returns(() => user);

            // ACT
            var updatedTask = service.Update(user);

            // ASSERT
            _userRep.Verify(repo => repo.Update(It.Is<User>(t => t == user)), Times.Once);
        }
        [Fact]
        public void UpdateUserWithInvalidInputTest()
        {
            // ARRANGE
            var user = new User()
            {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
            };

            var service = new UserService(_userRep.Object);

            _userRep.Setup(repo => repo.Get(It.Is<int>(z => z == user.UserID))).Returns(() => user);

            // ACT & ASSERT
            Assert.Throws<EntityDataMissingException>(() => service.Update(user));
            _userRep.Verify(repo => repo.Create(user), Times.Never);
        }
        [Fact]
        public void UpdateUserWithNullInputTest()
        {
            // ARRANGE
            User user = null;

            var service = new UserService(_userRep.Object);

            _userRep.Setup(repo => repo.Get(It.Is<int>(z => z == user.UserID))).Returns(() => user);

            // ACT & ASSERT
            Assert.Throws<NullReferenceException>(() => service.Update(user));
            _userRep.Verify(repo => repo.Create(user), Times.Never);
        }
        [Fact]
        public void DeleteUserTest()
        {
            // ARRANGE
            var user = new User()
            {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11 11 11 11",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
            };

            var service = new UserService(_userRep.Object);

            // check if existing
            _userRep.Setup(repo => repo.Get(It.Is<int>(t => t == user.UserID))).Returns(() => user);

            // ACT
            var deletedTask = service.Delete(user);

            // ASSERT
            _userRep.Verify(repo => repo.Delete(user), Times.Once);
            deletedTask.Should().BeNull();
        }
        [Fact]
        public void DeleteNonExistantUserTest()
        {
            // ARRANGE
            var user = new User()
            {
                Email = "test@tests.com",
                IsAdmin = true,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11 11 11 11",
                PasswordHash = new byte[10],
                PasswordSalt = new byte[5]
            };

            var service = new UserService(_userRep.Object);

            // set db response null
            _userRep.Setup(repo => repo.Get(It.Is<int>(t => t == user.UserID))).Returns(() => null);

            //ACT & ASSERT
            Assert.Throws<EntityDataMissingException>(() => service.Delete(user));
            _userRep.Verify(repo => repo.Delete(user), Times.Never);
        }

        #endregion
    }
}
