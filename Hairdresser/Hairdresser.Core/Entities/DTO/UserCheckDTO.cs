using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities.DTO
{
    public class UserCheckDTO
    {
        public int? UserID { get; set; }
        public string PhoneNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Boolean? IsAdmin { get; set; }
    }
}
