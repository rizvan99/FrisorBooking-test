using Hairdresser.Core.Application.Implementation;
using Hairdresser.Core.Application.Interfaces;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace Hairdresser.SpecFlow.StepDefinitions
{
    [Binding]
    public class CreateAppointmentStepDefinitions
    {

        private IAppointmentRepository appointmentRepo;
        private AppointmentService appointmentService;

        Appointment appointment = new Appointment();
        Appointment resultAppointment = new Appointment();
        Exception _actualException;

        public CreateAppointmentStepDefinitions()
        {
            var mock = new Mock<IAppointmentRepository>();
            mock.Setup(_ => _.Create(It.IsAny<Appointment>())).Returns(new Appointment());

            appointmentRepo = mock.Object;
            
            appointmentService = new AppointmentService(appointmentRepo);
        }

        // Standard appointment we can call and use for our tests
        private void SetAppointment()
        {
            appointment = new Appointment()
            {
                AppointmentDateTime = DateTime.Today,
                CustomerID = 1,
                EmployeeID = 1,
                Status = 1,
                TreatmentID = 1,
            };
        }

        [Given(@"the appointment date is (.*)")]
        public void GivenTheAppointmentDateIsAssignmentDate_AddDays(int value)
        {
            SetAppointment();
            appointment.AppointmentDateTime = DateTime.Today.AddDays(value);
        }

        [When(@"we call CreateAppointment")]
        public void WhenWeCallCreateAppointment()
        {
            //var service = new AppointmentService(appointmentRepo.Object);
            try
            {
                resultAppointment = appointmentService.Create(appointment);
                Console.WriteLine(resultAppointment);
            }
            catch (Exception e)
            {
                _actualException = e;
            }
            
        }

        [Then(@"the outcome should be (.*)")]
        public void ThenTheOutcomeShouldBeTrue(bool result)
        {
            bool isSame = false;
            if(resultAppointment is Appointment)
            {
                isSame = true;
            }
            Assert.Equal(isSame, result);
        }

        [Then(@"the outcomse should throw an (.*)")]
        public void ThenTheOutcomseShouldThrowAn(string p0)
        {
            var something = new object();
            Assert.Equal(p0, _actualException.Message);
        }

    }
}
