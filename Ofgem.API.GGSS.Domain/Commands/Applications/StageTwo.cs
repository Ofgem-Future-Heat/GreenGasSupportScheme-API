using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using System;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class StageTwo : IRequest<ApplicationResponse>
    {
        public Guid ApplicationId { get; set; }
    }
}
