using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string ProviderId { get; set; }
        public UserValue Value { get; set; }
        public bool IsResponsiblePerson { get; set; }
    }
}
