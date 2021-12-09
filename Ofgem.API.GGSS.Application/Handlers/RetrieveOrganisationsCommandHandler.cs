using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class RetrieveOrganisationsCommandHandler : IRequestHandler<RetrieveOrganisations, RetrieveOrganisationsResponse>
    {
        private readonly IOrganisationRepository _repository;

        public RetrieveOrganisationsCommandHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        public async Task<RetrieveOrganisationsResponse> Handle(RetrieveOrganisations request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllOrganisations(cancellationToken);

            if (result == null)
            {
                return new RetrieveOrganisationsResponse()
                {
                    Errors = new List<string>()
                    {
                        "ORGANISATION_NOT_FOUND"
                    }
                };
            }
            
            return new RetrieveOrganisationsResponse
            {
                Organisations = result.Select(o => new GetOrganisations()
                {
                    OrganisationId = o.Id.ToString(),
                    OrganisationName = o.Value?.Name,
                    OrganisationStatus = o.Value?.OrganisationStatus,
                    LastModified = o.Value?.LastModified ?? DateTime.Now.ToString("s")
                }).ToList()
            };
        }
    }
}