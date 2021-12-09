using Ofgem.API.GGSS.Domain.Contracts.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Models
{
    public class OrganisationModel : ISerializableEntity<OrganisationValue>
    {
        public string Id { get; set; }
        public OrganisationValue Value { get; set; }
        public List<ResponsiblePersonModel> ResponsiblePeople { get; set; }        
    }
}
