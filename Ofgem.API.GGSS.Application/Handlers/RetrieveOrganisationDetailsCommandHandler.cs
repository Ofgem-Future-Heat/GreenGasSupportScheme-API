using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.Application.Handlers
{
    
    public class RetrieveOrganisationDetailsCommandHandler : IRequestHandler<RetrieveOrganisationDetails, RetrieveOrganisationDetailsResponse>
    {
        private readonly IOrganisationRepository _repository;

        public RetrieveOrganisationDetailsCommandHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<RetrieveOrganisationDetailsResponse> Handle(RetrieveOrganisationDetails request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetOrganisationDetailsByOrgId(request.OrganisationId, cancellationToken);

            if (result == null)
            {
                return new RetrieveOrganisationDetailsResponse()
                {
                    Errors = new List<string>()
                    {
                        "ORGANISATION_DETAILS_NOT_FOUND"
                    }
                };
            }

            return new RetrieveOrganisationDetailsResponse()
            {
                OrganisationType = result.Value?.Type.ToString(),
                OrganisationName = result.Value?.Name,
                OrganisationRegistrationNumber = result.Value?.RegistrationNumber,
                OrganisationStatus = result.Value?.OrganisationStatus,
                OrganisationAddress = result.Value?.RegisteredOfficeAddress,
                ResponsiblePersonName = result.ResponsiblePeople.FirstOrDefault()?.User.Value.Name,
                ResponsiblePersonSurname = result.ResponsiblePeople.FirstOrDefault()?.User.Value.Surname,
                ResponsiblePersonPhoneNumber = result.ResponsiblePeople.FirstOrDefault()?.Value.TelephoneNumber,
                ResponsiblePersonEmail = result.ResponsiblePeople.FirstOrDefault()?.User.Value.EmailAddress,
                PhotoId = result.Value?.PhotoId,
                ProofOfAddress = result.Value?.ProofOfAddress,
                LetterOfAuthority = result.Value?.LetterOfAuthorisation,
                LegalDocument = result.Value?.LegalDocument
            };
        }
    }
}