using MediatR;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using System.Linq;
using System.Security.Claims;

namespace Ofgem.API.GGSS.Domain.Commands.Organisations
{
    public class OrganisationNominationSave : IRequest<OrganisationResponse>
    {
        public string ProviderId { get; private set; }
        public OrganisationNomineeModel Model { get; set; }
        public OrganisationNominationSave(ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(this.ProviderId))
                this.ProviderId = user?.Claims?.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
