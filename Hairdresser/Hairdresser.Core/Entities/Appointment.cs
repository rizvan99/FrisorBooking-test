using Hairdresser.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities
{
    public class Appointment
    {
        public DateTime AppointmentDateTime { get; set; }
        public int CustomerID { get; set; }
        public User Customer { get; set; }
        public int TreatmentID { get; set; }
        public Treatment Treatment { get; set; }
        public int EmployeeID { get; set; }
        public User Employee { get; set; }
        public int Status { get; set; }
    }
}
