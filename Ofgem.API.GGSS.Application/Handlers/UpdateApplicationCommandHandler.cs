using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplication, UpdateApplicationResponse>
    {
        private readonly IApplicationRepository _applicationRepository;

        public UpdateApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<UpdateApplicationResponse> Handle(UpdateApplication request, CancellationToken cancellationToken)
        {
            var response = new UpdateApplicationResponse();
            
            var storedApplication = await _applicationRepository.GetByIdAsync(Guid.Parse(request.ApplicationId));

            if (storedApplication == null)
            {
                response.Errors.Add("APPLICATION_NOT_FOUND");
                return response;
            }
            
            storedApplication.Value = request.Application;
            storedApplication.Value.LastModified = DateTime.Now.ToString("s");
            
            await _applicationRepository.UpdateAsync(storedApplication, Guid.Parse(request.UserId), cancellationToken);

            return response;
        }
    }
}