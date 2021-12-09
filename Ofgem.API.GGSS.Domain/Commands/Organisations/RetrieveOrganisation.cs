using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Domain.Commands.Organisations
{
    public class RetrieveOrganisation : IRequest<RetrieveOrganisationResponse>
    {
        public string OrganisationId { get; set; }
    }
}