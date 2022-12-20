using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Exceptions.Service.General
{
    public class ListIsNullOrEmptyException : Exception
    {
        public ListIsNullOrEmptyException()
        {
        }
        public ListIsNullOrEmptyException(string message)
            : base(message)
        {
        }
        public ListIsNullOrEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
