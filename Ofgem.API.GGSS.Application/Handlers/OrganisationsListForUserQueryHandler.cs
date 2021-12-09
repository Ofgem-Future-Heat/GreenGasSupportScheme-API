using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.Queries.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class OrganisationsListForUserQueryHandler : IRequestHandler<OrganisationsListForUser, List<OrganisationModel>>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IMapper _mapper;

        public OrganisationsListForUserQueryHandler(IMapper mapper, IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository ?? throw new ArgumentNullException(nameof(organisationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<OrganisationModel>> Handle(OrganisationsListForUser request, CancellationToken cancellationToken)
        {
            var data = (await _organisationRepository.ListAllForUserAsync(request.ProviderId)).OrderBy(x => x.Value.Name);
            return _mapper.Map<List<OrganisationModel>>(data);
        }
    }
}
