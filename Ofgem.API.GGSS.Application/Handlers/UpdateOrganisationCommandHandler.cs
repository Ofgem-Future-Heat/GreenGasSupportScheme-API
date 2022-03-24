using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class UpdateOrganisationCommandHandler : IRequestHandler<UpdateOrganisation, UpdateOrganisationResponse>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public UpdateOrganisationCommandHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<UpdateOrganisationResponse> Handle(UpdateOrganisation request, CancellationToken cancellationToken)
        {
            var updateOrganisationResponse = new UpdateOrganisationResponse();
            
            var organisation = await _organisationRepository.GetByIdAsync(Guid.Parse(request.OrganisationId));

            if (organisation == null)
            {
                updateOrganisationResponse.Errors.Add("ORGANISATION_NOT_FOUND");

                return updateOrganisationResponse;
            }

            organisation.Value.OrganisationStatus = request.OrganisationStatus;
            organisation.Value.LastModified = DateTime.Now.ToString("s");

            Guid userId = request.UserId == null ? default : Guid.Parse(request.UserId);
            await _organisationRepository.UpdateAsync(organisation, userId, cancellationToken);
            
            return updateOrganisationResponse;
        }
    }

    public class UpdateOrganisation : IRequest<UpdateOrganisationResponse>
    {
        public string OrganisationId { get; set; }
        public string OrganisationStatus { get; set; }
        public string UserId { get; set; }
    }
    
    public class UpdateOrganisationResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}