using Hairdresser.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Application.Interfaces
{
    public interface ILoginService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        string GenerateToken(User user);
    }
}
