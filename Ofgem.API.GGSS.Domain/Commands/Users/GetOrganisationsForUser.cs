using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using Ofgem.API.GGSS.Domain.Responses.Users;

namespace Ofgem.API.GGSS.Domain.Commands.Users
{
    public class GetOrganisationsForUser : IRequest<GetOrganisationsForUserResponse>
    {
        public string UserId { get; set; }
    }
}