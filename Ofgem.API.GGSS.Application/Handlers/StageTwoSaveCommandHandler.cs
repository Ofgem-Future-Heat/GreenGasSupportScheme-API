using MediatR;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class StageTwoSaveCommandHandler : IRequestHandler<ApplicationSave<StageTwo>, ApplicationResponse>
    {
        public StageTwoSaveCommandHandler()
        {

        }

        public Task<ApplicationResponse> Handle(ApplicationSave<StageTwo> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
