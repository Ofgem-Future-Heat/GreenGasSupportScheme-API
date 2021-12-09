using Ofgem.API.GGSS.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.GGSS.Domain.Attributes
{
    public class ValidOrganisationTypeAttribute : ValidationAttribute
    {
        public ValidOrganisationTypeAttribute() {}
        public override bool IsValid(object value)
        {
            if (value == null) return true; // Nullable
            return Enum.IsDefined(typeof(OrganisationType), value);
        }
    }
}
