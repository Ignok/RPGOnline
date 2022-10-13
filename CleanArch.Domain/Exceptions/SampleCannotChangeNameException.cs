using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Domain.Exceptions
{
    public class SampleCannotChangeNameException : Exception
    {
        public SampleCannotChangeNameException()
        {
        }

        public SampleCannotChangeNameException(string? message) : base(message)
        {
        }

        public SampleCannotChangeNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SampleCannotChangeNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
