using System;
using Ofgem.API.GGSS.Domain.Attributes;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Models;

namespace Ofgem.API.GGSS.Domain.ModelValues
{
    public class OrganisationValue
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public AddressModel RegisteredOfficeAddress { get; set; }
        [ValidOrganisationType(ErrorMessage = "Can be 'Private' or 'Other' only.")]
        public OrganisationType? Type { get; set; }
        public DocumentValue LegalDocument { get; set; }
        public DocumentValue LetterOfAuthorisation { get; set; }
        public DocumentValue PhotoId { get; set; }
        public DocumentValue ProofOfAddress { get; set; }
        public string Error { get; set; }
        public string OrganisationStatus { get; set; } = "Not verified";
        public string LastModified { get; set; }
    }
}
