using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class OrganisationDelegateSaveCommandHandler : IRequestHandler<OrganisationNominationSave, OrganisationResponse>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IMapper _mapper;

        public OrganisationDelegateSaveCommandHandler(IMapper mapper, IOrganisationRepository organisationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organisationRepository = organisationRepository ?? throw new ArgumentNullException(nameof(organisationRepository));      
        }

        public async Task<OrganisationResponse> Handle(OrganisationNominationSave request, CancellationToken cancellationToken)
        {
            var value = _mapper.Map<OrganisationValue>(request.Model);
            value.ReferenceNumber = Guid.NewGuid().ToString();
            var organisation = new Organisation { Value = value };
            var resultAsync = await _organisationRepository.AddAsync(organisation, Guid.Parse(request.ProviderId), cancellationToken)
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
