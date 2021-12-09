using System;
using System.Runtime.Serialization;

namespace Ofgem.API.GGSS.Application.Exceptions
{
    [Serializable]
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() { }

        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }

        // Without this constructor, deserialization will fail
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
