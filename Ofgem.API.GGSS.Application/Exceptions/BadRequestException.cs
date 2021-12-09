using System;
using System.Runtime.Serialization;

namespace Ofgem.API.GGSS.Application.Exceptions
{
    [Serializable]
    public class BadRequestException : ApplicationException
    {
        public BadRequestException() { }

        public BadRequestException(string message) : base(message) { }

        // Without this constructor, deserialization will fail
        protected BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
