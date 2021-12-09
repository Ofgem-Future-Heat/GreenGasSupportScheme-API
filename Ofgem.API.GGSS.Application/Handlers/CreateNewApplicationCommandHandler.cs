using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class CreateNewApplicationCommandHandler : IRequestHandler<CreateNewApplication, CreateNewApplicationResponse>
    {
        private readonly IApplicationRepository _applicationRepository;

        public CreateNewApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<CreateNewApplicationResponse> Handle(CreateNewApplication request,
            CancellationToken cancellationToken)
        {
            var application = new Entities.Application()
            {
                OrganisationId = new Guid(request.OrganisationId),
                Value = new ApplicationValue() { LastModified = DateTime.Now.ToString("s") }
            };
            
            var result = await _applicationRepository.AddAsync(application, Guid.Parse(request.UserId), cancellationToken);

            return new CreateNewApplicationResponse()
            {
                NewApplicationId = result.Id.ToString()
            };
        }
    }
}