using FluentAssertions;
using Hairdresser.Core.Application.Implementation;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Core.Exceptions.Service.General;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Hairdresser.XUnitTests
{
    public class AppointmentServiceXUnitTests
    {
        private List<Appointment> _appointments = null;
        private readonly Mock<IAppointmentRepository> _AppointmentRep;
        public AppointmentServiceXUnitTests()
        {
            _AppointmentRep = new Mock<IAppointmentRepository>();
            _AppointmentRep.Setup(repo => repo.Read()).Returns(() => _appointments);
        }
        #region crudTests
        [Fact]
        public void CreateAppointmentWithValidInputTest()
        {
            //Arrange
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddHours(3),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0
            };

            var service = new AppointmentService(_AppointmentRep.Object);

            _appointments = new List<Appointment>();

            //Act
            var result = service.Create(appointment);
            _appointments.Add(appointment);

            //Assert
            _AppointmentRep.Setup(repo => repo.Create(appointment)).Returns(result);
            _AppointmentRep.Verify(repo => repo.Create(appointment), Times.Once);
            _appointments.Should().Contain(appointment);
        }

        [Fact]
        public void CreateAppointmentWithInvalidInputTest()
        {
            //Arrange
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddHours(1),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 0,
                Status = 0
            };

            var service = new AppointmentService(_AppointmentRep.Object);
            _appointments = new List<Appointment>();

            //Act & Assert
            Assert.Throws<EntityDataMissingException>(() => service.Create(appointment));
            _AppointmentRep.Verify(repo => repo.Create(appointment), Times.Never);
        }

        [Fact]
        public void CreateAppointmentWithDateInPast_ThrowException()
        {
            // Arrange
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(-1),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0,
            };

            var service = new AppointmentService(_AppointmentRep.Object);

            //Act & Assert
            Assert.Throws<ArgumentException>(() => service.Create(appointment));
            _AppointmentRep.Verify(repo => repo.Create(appointment), Times.Never);
        }

        [Fact]
        public void CreateAppointmentWithDateOverOneYear_ThrowException()
        {
            // Arrange
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddYears(1).AddDays(1),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0,
            };

            var service = new AppointmentService(_AppointmentRep.Object);

            //Act & Assert
            Assert.Throws<EntityDataMissingException>(() => service.Create(appointment));
            _AppointmentRep.Verify(repo => repo.Create(appointment), Times.Never);
        }


        [Fact]
        public void CreateAppointmentWithNullInputTest()
        {
            //Arrange
            Appointment user = null;

            var service = new AppointmentService(_AppointmentRep.Object);
            _appointments = new List<Appointment>();

            //Act & Assert
            Assert.Throws<NullReferenceException>(() => service.Create(user));
            _AppointmentRep.Verify(repo => repo.Create(user), Times.Never);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllAppointmentsTest(int listCount)
        {
            // ARRANGE
            var _users = new List<Appointment>()
            {
                new Appointment()
                {
                AppointmentDateTime = DateTime.Now,
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0
                },
                new Appointment()
                {
                AppointmentDateTime = DateTime.Now.AddDays(1),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0
                }
        };

            var service = new AppointmentService(_AppointmentRep.Object);

            _AppointmentRep.Setup(repo => repo.Read()).Returns(() => _users.GetRange(0, listCount));

            // ACT
            var tasksFound = service.Read();

            // ASSERT
            Assert.Equal(_users.GetRange(0, listCount), tasksFound);
            _AppointmentRep.Verify(repo => repo.Read(), Times.Once);
        }
        [Fact]
        public void GetAllWithNoAppointmentsTest()
        {
            // ARRANGE
            var _users = new List<Appointment>();
            var service = new AppointmentService(_AppointmentRep.Object);

            // ACT & ASSERT
            Assert.Throws<ListIsNullOrEmptyException>(() => service.Read());
            _AppointmentRep.Verify(repo => repo.Read(), Times.Once);
        }
        [Fact]
        public void UpdateAppointmentWithValidInputTest()
        {
            // ARRANGE
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddHours(3),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0
            };

            var service = new AppointmentService(_AppointmentRep.Object);

            _AppointmentRep.Setup(repo => repo.Get(It.Is<int>(z => z == appointment.EmployeeID), It.Is<DateTime>(d => d.CompareTo(appointment.AppointmentDateTime) == 0))).Returns(() => appointment);

            // ACT
            var updatedTask = service.Update(appointment);

            // ASSERT
            _AppointmentRep.Verify(repo => repo.Update(It.Is<Appointment>(t => t == appointment)), Times.Once);
        }
        [Fact]
        public void UpdateAppointmentWithInvalidInputTest()
        {
            // ARRANGE
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddHours(1),
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 0,
                Status = 0
            };

            var service = new AppointmentService(_AppointmentRep.Object);

            _AppointmentRep.Setup(repo => repo.Get(It.Is<int>(z => z == appointment.EmployeeID), It.Is<DateTime>(d => d.CompareTo(appointment.AppointmentDateTime) == 0))).Returns(() => appointment);

            // ACT & ASSERT
            Assert.Throws<EntityDataMissingException>(() => service.Update(appointment));
            _AppointmentRep.Verify(repo => repo.Create(appointment), Times.Never);
        }
        [Fact]
        public void UpdateAppointmentWithNullInputTest()
        {
            // ARRANGE
            Appointment appointment = null;

            var service = new AppointmentService(_AppointmentRep.Object);

            _AppointmentRep.Setup(repo => repo.Get(It.Is<int>(z => z == appointment.EmployeeID), It.Is<DateTime>(d => d.CompareTo(appointment.AppointmentDateTime) == 0))).Returns(() => appointment);

            // ACT & ASSERT
            Assert.Throws<NullReferenceException>(() => service.Update(appointment));
            _AppointmentRep.Verify(repo => repo.Create(appointment), Times.Never);
        }

        [Fact]
        public void DeleteNonExistantAppointmentTest()
        {
            // ARRANGE
            var appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Now,
                CustomerID = 1,
                EmployeeID = 2,
                TreatmentID = 1,
                Status = 0
            };

            var service = new AppointmentService(_AppointmentRep.Object);

            // set db response null
            _AppointmentRep.Setup(repo => repo.Get(It.Is<int>(z => z == appointment.EmployeeID), It.Is<DateTime>(d => d.CompareTo(appointment.AppointmentDateTime) == 0))).Returns(() => null);

            //ACT & ASSERT
            Assert.Throws<ReplyIsNullOrEmptyException>(() => service.Delete(appointment));
            _AppointmentRep.Verify(repo => repo.Delete(appointment), Times.Once);
        }

        #endregion
    }
}
