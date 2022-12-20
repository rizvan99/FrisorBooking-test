using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Exceptions.Service.General
{
    public class EntityDataMissingException : Exception
    {
        public EntityDataMissingException()
        {
        }
        public EntityDataMissingException(string message)
            : base(message)
        {
        }
        public EntityDataMissingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
