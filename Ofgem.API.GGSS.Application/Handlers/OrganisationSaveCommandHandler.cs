using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class OrganisationSaveCommandHandler : IRequestHandler<OrganisationSave, OrganisationResponse>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IMapper _mapper;

        public OrganisationSaveCommandHandler(IMapper mapper, IOrganisationRepository organisationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organisationRepository = organisationRepository ?? throw new ArgumentNullException(nameof(organisationRepository));           
        }

        public async Task<OrganisationResponse> Handle(OrganisationSave request, CancellationToken cancellationToken)
        {
            var organisation = _mapper.Map<Organisation>(request.Model);

            var resultAsync = await _organisationRepository.AddAsync(organisation, Guid.Parse(request.UserId), cancellationToken)
                .ContinueWith(async saveAsync =>
                {
                    var org = await saveAsync;
                    var response = _mapper.Map<OrganisationResponse>(org);
                    response.ReferenceNumber = org.Value.ReferenceNumber;
                    return response;
                }, 
                TaskContinuationOptions.OnlyOnRanToCompletion);
            return await resultAsync;
        }
    }
}
