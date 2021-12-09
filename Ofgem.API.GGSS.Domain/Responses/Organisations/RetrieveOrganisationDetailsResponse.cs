using System.Collections.Generic;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Responses.Organisations
{
    public class RetrieveOrganisationDetailsResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public string OrganisationType { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationRegistrationNumber { get; set; }
        public AddressModel OrganisationAddress { get; set; }
        public string ResponsiblePersonName { get; set; }
        public string ResponsiblePersonSurname { get; set; }
        public string ResponsiblePersonPhoneNumber { get; set; }
        public string ResponsiblePersonEmail { get; set; }
        public DocumentValue PhotoId { get; set; }
        public DocumentValue ProofOfAddress { get; set; }
        public DocumentValue LetterOfAuthority { get; set; }
        public DocumentValue LegalDocument { get; set; }
        public string OrganisationStatus { get; set; }
    }
}