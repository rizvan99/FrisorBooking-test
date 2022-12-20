using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities
{
    public class CalendarDateModel
    {
        public DateTime AppointmentTime { get; set; }
        public Boolean Taken { get; set; }
    }
}
