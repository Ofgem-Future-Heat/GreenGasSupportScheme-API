using Ofgem.API.GGSS.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ofgem.API.GGSS.Domain.Attributes
{
    public class ValidLocationAttribute : ValidationAttribute
    {
        public ValidLocationAttribute() {}
        public override bool IsValid(object value)
        {
            return Enum.IsDefined(typeof(Location), value);
        }
    }
}
