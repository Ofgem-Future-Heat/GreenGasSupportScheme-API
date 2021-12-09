using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Models
{
    public partial class ApplicationModel : ISerializableEntity<ApplicationValue>
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public ApplicationValue Value { get; set; }
        public OrganisationModel Organisation { get; set; }    
        public List<DocumentModel> Documents { get; set; }
    }
}
