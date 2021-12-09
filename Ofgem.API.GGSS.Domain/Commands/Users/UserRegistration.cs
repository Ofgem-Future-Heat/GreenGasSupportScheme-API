using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Commands.Users
{
    public class UserRegistration
    {
        public string ProviderId { get; set; }
        public UserValue Value { get; set; }
        public bool IsResponsiblePerson { get; set; }
    }
}
