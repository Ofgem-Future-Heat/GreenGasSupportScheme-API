using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class StageOneSaveCommandHandler : IRequestHandler<ApplicationSave<StageOne>, string>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationRepository _applicationRepository;

        public StageOneSaveCommandHandler(IMapper mapper, IApplicationRepository applicationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
        }

        public async Task<string> Handle(ApplicationSave<StageOne> request, CancellationToken cancellationToken)
        {
            var newApplication = new Entities.Application
            {
                OrganisationId = request.Model.OrganisationId,
                Value = _mapper.Map<ApplicationValue>(request.Model),
                Documents = new List<Document>
                {
                    new Document{ Value = _mapper.Map<DocumentValue>(request.Model.PlanningPermissionDocument) },
                    new Document{ Value = _mapper.Map<DocumentValue>(request.Model.CapacityCheckDocument) }
                }
            };

            newApplication.Value.Status = Domain.Enums.ApplicationStatus.StageOneSubmitted;

            var resultAsymc = await _applicationRepository.AddAsync(newApplication, Guid.Parse(request.UserId), cancellationToken)
                .ContinueWith(async resultAsync =>
                {
                    var result = await resultAsync;
                    return result.Id.ToString();
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);

            return await resultAsymc;
        }
    }
}
