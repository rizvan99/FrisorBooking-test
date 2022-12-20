using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities.DTO
{
    public class PasswordWrapper
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
