using MediatR;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Domain.Commands.Organisations
{
    public class OrganisationSave : IRequest<string>, IRequest<OrganisationResponse>
    {
        public string UserId { get; set; }
        public OrganisationModel Model { get; set; }

        public OrganisationSave() { }
    }
}
