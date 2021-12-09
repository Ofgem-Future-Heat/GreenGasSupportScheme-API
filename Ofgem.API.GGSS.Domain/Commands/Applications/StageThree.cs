using MediatR;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using System;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class StageThree : IRequest<ApplicationResponse>
    {
        public Guid ApplicationId { get; set; }

        public DocumentValue FeedstockPlanDocument { get; set; }
    }
}
