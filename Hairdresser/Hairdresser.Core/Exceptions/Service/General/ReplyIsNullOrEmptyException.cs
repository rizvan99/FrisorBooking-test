using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Exceptions.Service.General
{
    public class ReplyIsNullOrEmptyException : Exception
    {
        public ReplyIsNullOrEmptyException()
        {
        }
        public ReplyIsNullOrEmptyException(string message)
            : base(message)
        {
        }
        public ReplyIsNullOrEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
