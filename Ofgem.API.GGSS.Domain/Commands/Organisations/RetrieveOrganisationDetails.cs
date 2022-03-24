using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Domain.Commands.Organisations
{
    public class RetrieveOrganisationDetails : IRequest<RetrieveOrganisationDetailsResponse>
    {
        public string OrganisationId { get; set; }
        public string UserId { get; set; }
    }
}