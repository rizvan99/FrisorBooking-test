using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Exceptions.Service.General
{
    public class EntityIsNullOrEmptyException : Exception
    {
        public EntityIsNullOrEmptyException()
        {
        }
        public EntityIsNullOrEmptyException(string message)
            : base(message)
        {
        }
        public EntityIsNullOrEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
