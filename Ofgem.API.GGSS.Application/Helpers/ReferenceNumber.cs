using System;
using System.Collections.Generic;
using System.Text;

namespace Ofgem.API.GGSS.Application.Helpers
{
    public class ReferenceNumber
    {
        protected ReferenceNumber() { }

        private const string SCHEME_ABBREVIATION = "GGSS";
        private const int LENGTH = 5;

        public static string GetApplicationReference(Guid applicationGuid, string reference)
        {
            return IsInvalid(reference)
                ? $"{SCHEME_ABBREVIATION}-{applicationGuid.ToString()[..LENGTH].ToUpper()}"
                : reference;
        }

        private static bool IsInvalid(string reference)
        {
            return (reference == null || reference.Length > LENGTH);
        }
    }
}
