using FluentAssertions;
using Hairdresser.Core.Application.Implementation;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Hairdresser.XUnitTests
{
    public class TreatmentServiceXUnitTests
    {
        private List<Treatment> _treatments = null;
        private readonly Mock<ICrudRepository<Treatment>> _serviceRp;
        public TreatmentServiceXUnitTests()
        {
            _serviceRp = new Mock<ICrudRepository<Treatment>>();
            _serviceRp.Setup(repo => repo.Read()).Returns(() => _treatments);
        }
        #region crudTests
        [Fact]
        public void CreateServiceWithValidInputTest()
        {
            //Arrange
            var serviceEntity = new Treatment()
            {
                TreatmentDuration = 30,
                TreatmentID = 1,
                TreatmentName = "testService",
                TreatmentPrice = 10.00
            };

            var treatment = new TreatmentService(_serviceRp.Object);

            _treatments = new List<Treatment>();

            //Act
            var result = treatment.Create(serviceEntity);
            _treatments.Add(serviceEntity);

            //Assert
            _serviceRp.Setup(repo => repo.Create(serviceEntity)).Returns(result);
            _serviceRp.Verify(repo => repo.Create(serviceEntity), Times.Once);
            _treatments.Should().Contain(serviceEntity);
        }

        [Fact]
        public void CreateServiceWithInvalidServicenameTest()
        {
            //Arrange
            var serviceEntity = new Treatment()
            {
                TreatmentDuration = 30,
                TreatmentID = 1,
                TreatmentName = "",
                TreatmentPrice = 10.00
            };

            var service = new TreatmentService(_serviceRp.Object);
            _treatments = new List<Treatment>();

            //Act & Assert
            Assert.Throws<ArgumentException>(() => service.Create(serviceEntity));
            _serviceRp.Verify(repo => repo.Create(serviceEntity), Times.Never);
        }
        [Fact]
        public void CreateServiceWithNullInputTest()
        {
            //Arrange
            Treatment serviceEntity = null;

            var treatment = new TreatmentService(_serviceRp.Object);
            _treatments = new List<Treatment>();

            //Act & Assert
            Assert.Throws<NullReferenceException>(() => treatment.Create(serviceEntity));
            _serviceRp.Verify(repo => repo.Create(serviceEntity), Times.Never);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllServicesTest(int listCount)
        {
            // ARRANGE
            var _users = new List<Treatment>()
            {
                new Treatment()
                {
                TreatmentDuration = 30,
                TreatmentID= 1,
                TreatmentName = "testService",
                TreatmentPrice = 10.00
                },
                new Treatment()
                {
                TreatmentDuration = 30,
                TreatmentID = 2,
                TreatmentName = "testService2",
                TreatmentPrice = 10.00
                }
        };

            var treatment = new TreatmentService(_serviceRp.Object);

            _serviceRp.Setup(repo => repo.Read()).Returns(() => _users.GetRange(0, listCount));

            // ACT
            var tasksFound = treatment.Read();

            // ASSERT
            Assert.Equal(_users.GetRange(0, listCount), tasksFound);
            _serviceRp.Verify(repo => repo.Read(), Times.Once);
        }
        [Fact]
        public void GetAllWithNoServicesTest()
        {
            // ARRANGE
            var _users = new List<Treatment>();
            var service = new TreatmentService(_serviceRp.Object);

            // ACT & ASSERT
            Assert.Throws<NullReferenceException>(() => service.Read());
            _serviceRp.Verify(repo => repo.Read(), Times.Once);
        }
        [Fact]
        public void UpdateServiceWithValidInputTest()
        {
            // ARRANGE
            var serviceEntity = new Treatment()
            {
                TreatmentDuration = 30,
                TreatmentID = 1,
                TreatmentName = "testService",
                TreatmentPrice = 10.00
            };

            var service = new TreatmentService(_serviceRp.Object);

            _serviceRp.Setup(repo => repo.Get(It.Is<int>(z => z == serviceEntity.TreatmentID))).Returns(() => serviceEntity);

            // ACT
            var updatedTask = service.Update(serviceEntity);

            // ASSERT
            _serviceRp.Verify(repo => repo.Update(It.Is<Treatment>(t => t == serviceEntity)), Times.Once);
        }
        [Fact]
        public void UpdateServiceWithInvalidInputTest()
        {
            // ARRANGE
            var serviceEntity = new Treatment()
            {
                TreatmentDuration = 30,
                TreatmentID = 1,
                TreatmentName = "",
                TreatmentPrice = 10.00
            };

            var service = new TreatmentService(_serviceRp.Object);

            _serviceRp.Setup(repo => repo.Get(It.Is<int>(z => z == serviceEntity.TreatmentID))).Returns(() => serviceEntity);

            // ACT & ASSERT
            Assert.Throws<ArgumentException>(() => service.Update(serviceEntity));
            _serviceRp.Verify(repo => repo.Create(serviceEntity), Times.Never);
        }
        [Fact]
        public void UpdateServiceWithNullInputTest()
        {
            // ARRANGE
            Treatment serviceEntity = null;

            var service = new TreatmentService(_serviceRp.Object);

            _serviceRp.Setup(repo => repo.Get(It.Is<int>(z => z == serviceEntity.TreatmentID))).Returns(() => serviceEntity);

            // ACT & ASSERT
            Assert.Throws<NullReferenceException>(() => service.Update(serviceEntity));
            _serviceRp.Verify(repo => repo.Create(serviceEntity), Times.Never);
        }
        [Fact]
        public void DeleteServiceTest()
        {
            // ARRANGE
            var serviceEntity = new Treatment()
            {
                TreatmentDuration = 30,
                TreatmentID = 1,
                TreatmentName = "testService",
                TreatmentPrice = 10.00
            };

            var service = new TreatmentService(_serviceRp.Object);

            // check if existing
            _serviceRp.Setup(repo => repo.Get(It.Is<int>(t => t == serviceEntity.TreatmentID))).Returns(() => serviceEntity);

            // ACT
            var deletedTask = service.Delete(serviceEntity);

            // ASSERT
            _serviceRp.Verify(repo => repo.Delete(serviceEntity), Times.Once);
            deletedTask.Should().BeNull();
        }
        [Fact]
        public void DeleteNonExistantServiceTest()
        {
            // ARRANGE
            var serviceEntity = new Treatment()
            {
                TreatmentDuration = 30,
                TreatmentID = 1,
                TreatmentName = "testService",
                TreatmentPrice = 10.00
            };

            var service = new TreatmentService(_serviceRp.Object);

            // set db response null
            _serviceRp.Setup(repo => repo.Get(It.Is<int>(t => t == serviceEntity.TreatmentID))).Returns(() => null);

            //ACT & ASSERT
            Assert.Throws<ArgumentException>(() => service.Delete(serviceEntity));
            _serviceRp.Verify(repo => repo.Delete(serviceEntity), Times.Never);
        }

        #endregion
    }
}
