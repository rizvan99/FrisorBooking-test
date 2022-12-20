using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities.Auth
{
    public class RegisterInputModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
