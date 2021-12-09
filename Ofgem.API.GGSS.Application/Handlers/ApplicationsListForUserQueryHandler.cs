using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.Queries.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class ApplicationsListForUserQueryHandler : IRequestHandler<ApplicationsListForUser, List<ApplicationModel>>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationsListForUserQueryHandler(IMapper mapper, IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ApplicationModel>> Handle(ApplicationsListForUser request, CancellationToken cancellationToken)
        {
            var data = request.IncludeDocuments
                ? (await _applicationRepository.ListAllForUserWithDocumentsAsync(Guid.Parse(request.UserProviderId))).OrderBy(x => x.Value.DateInjectionStart)
                : (await _applicationRepository.ListAllForUserAsync(Guid.Parse(request.UserProviderId))).OrderBy(x => x.Value.DateInjectionStart);
            return _mapper.Map<List<ApplicationModel>>(data);
        }
    }
}
