using MediatR;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class StageThreeSaveCommandHandler : IRequestHandler<ApplicationSave<StageThree>, ApplicationResponse>
    {
        public async Task<ApplicationResponse> Handle(ApplicationSave<StageThree> request, CancellationToken cancellationToken)
        {
            throw await Task.FromResult(new NotImplementedException());
        }
    }
}
