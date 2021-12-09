using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class ResponsiblePersonNomineeModel : ISerializableEntity<ResponsiblePersonValue>
    {        
        public string EmailAddress { get; set; }
        public DocumentValue AuthorityLetterFile { get; set; }
        public ResponsiblePersonValue Value { get; set; }
    }
}
