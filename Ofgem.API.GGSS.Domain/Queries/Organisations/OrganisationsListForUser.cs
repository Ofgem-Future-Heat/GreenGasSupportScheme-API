using MediatR;
using Ofgem.API.GGSS.Domain.Models;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Queries.Organisations
{
    public class OrganisationsListForUser : IRequest<List<OrganisationModel>>
    {
        public string ProviderId { get; set; }
    }
}
