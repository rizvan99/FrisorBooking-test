using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Filter
{
    public class AppointmentFilter
    {
        public Filter Filter { get; set; }
        public Boolean OnlyActiveAppointments { get; set; }
        public int FilterEmployeeId { get; set; }
        public Boolean ShowOld { get; set; }
        public Boolean ShowCancelled { get; set; }
    }
}
