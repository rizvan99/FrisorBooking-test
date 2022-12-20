using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities.DTO
{
    public class AppointmentDTO
    {
        public DateTime AppointmentDateTime { get; set; }

        public int Status { get; set; }

        public UserBasicDTO Customer { get; set; }

        public UserBasicDTO Employee { get; set; }

        public Treatment Treatment { get; set; }
        public object Service { get; set; }
    }
}
