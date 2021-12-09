using System;
using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class CreateNewApplication : IRequest<CreateNewApplicationResponse>
    {
        public string OrganisationId { get; set; }
        
        public string UserId { get; set; }
    }
}