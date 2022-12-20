using Hairdresser.Core.Entities;
using Hairdresser.Core.Entities.DTO;
using System;

namespace Hairdresser.WebAPI.DTO
{
    public class AppointmentDto
    {
        public DateTime AppointmentDateTime { get; set; }

        public int Status { get; set; }

        public UserBasicDTO Customer { get; set; }

        public UserBasicDTO Employee { get; set; }

        public Treatment Treatment { get; set; }
    }
}
