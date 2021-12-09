using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class UpdateApplicationRequestModel
    {
        public  ApplicationValue Application { get; set; }

        public string UserId { get; set; }
    }
}