using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Helpers;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class RetrieveApplicationsCommandHandler : IRequestHandler<RetrieveApplications, RetrieveApplicationsResponse>
    {
        private readonly IApplicationRepository _applicationRepository;

        public RetrieveApplicationsCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<RetrieveApplicationsResponse> Handle(RetrieveApplications request, CancellationToken cancellationToken)
        {
            var applications = await _applicationRepository.ListAll(cancellationToken);

            var response = new RetrieveApplicationsResponse() { List = new List<GetApplications>() };

            if (applications != null)
            {
                response.List =
                    applications
                        .Select(a => new GetApplications()
                        {
                            ApplicationId = a.Id.ToString(),
                            OrganisationName = a.Organisation.Value.Name,
                            ApplicationStatus = MapApplicationStatusHelper.GetMappedStatus(a).ToString(),
                            LastModified = a.Value.LastModified ?? DateTime.Now.ToString("s"),
                            Reference = ReferenceNumber.GetApplicationReference(a.Id, a.Value.Reference)
                        })
                        .ToList();
            }
            else
            {
                response.Errors.Add("APPLICATIONS_NOT_FOUND");
            }

            return response;
        }
    }
}
