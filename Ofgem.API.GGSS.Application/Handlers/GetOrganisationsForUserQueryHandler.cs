using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Users;
using Ofgem.API.GGSS.Domain.Responses.Users;

namespace Ofgem.API.GGSS.Application.Handlers.Users
{
    public class GetOrganisationsForUserQueryHandler : IRequestHandler<GetOrganisationsForUser, GetOrganisationsForUserResponse>
    {
        private readonly IOrganisationRepository _repository;

        public GetOrganisationsForUserQueryHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetOrganisationsForUserResponse> Handle(GetOrganisationsForUser request, CancellationToken cancellationToken)
        {
            var result = await _repository.ListAllForUserAsync(request.UserId, cancellationToken);
            if (result == null)
            {
                return new GetOrganisationsForUserResponse()
                {
                    Errors = new List<string>()
                    {
                        "USER_NOT_FOUND"
                    }
                };
            }
            return new GetOrganisationsForUserResponse
            {
                Organisations = result.Select(o => new GetOrganisationForUser()
                {
                    OrganisationId = o.Id.ToString(),
                    OrganisationName = o.Value?.Name,
                    NumberOfApplications = o.Applications.Count
                }).ToList()
            };
        }
    }
}