using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class RetrieveOrganisationCommandHandler : IRequestHandler<RetrieveOrganisation, RetrieveOrganisationResponse>
    {
        private readonly IOrganisationRepository _repository;

        public RetrieveOrganisationCommandHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        public async Task<RetrieveOrganisationResponse> Handle(RetrieveOrganisation request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdWithApplications(request.OrganisationId, cancellationToken);
            if (result == null)
            {
                return new RetrieveOrganisationResponse()
                {
                    Errors = new List<string>()
                    {
                        "ORGANISATION_NOT_FOUND"
                    }
                };
            }
            return new RetrieveOrganisationResponse
            {
                Name = result.Value.Name,

                Applications = result.Applications.Select(a => new RetrieveOrganisationApplicationResponse()
                {
                    Id = a.Id.ToString(),
                    Name = a.Value.StageOne.TellUsAboutYourSite.PlantName,
                    Status = a.Value.Status.ToString()
                }).ToList()
            };
        }
    }
}