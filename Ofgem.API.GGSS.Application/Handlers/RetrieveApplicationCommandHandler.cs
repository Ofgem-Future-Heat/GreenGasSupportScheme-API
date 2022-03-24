using System;
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
    public class RetrieveApplicationCommandHandler : IRequestHandler<RetrieveApplication, RetrieveApplicationResponse>
    {
        private readonly IApplicationRepository _applicationRepository;

        public RetrieveApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }


        public async Task<RetrieveApplicationResponse> Handle(RetrieveApplication request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetById(Guid.Parse(request.ApplicationId), cancellationToken);

            var response = new RetrieveApplicationResponse();

            if (application != null)
            {
                application.Value.Status = MapApplicationStatusHelper.GetMappedStatus(application);
                response.Application = application.Value;
                response.OrganisationId = application.OrganisationId;
                response.Application.Reference = ReferenceNumber.GetApplicationReference(application.Id, application.Value.Reference);
                response.Application.CanSubmit =
                    application.Organisation.ResponsiblePeople.Any(rp => rp.UserId.ToString() == request.UserId);
            }
            else
            {
                response.Errors.Add("APPLICATION_NOT_FOUND");
            }

            return response;
        }
    }
}