using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities.Auth
{
    public class User
    {
        public int UserID { get; set; }
        public Boolean IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
