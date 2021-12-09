using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Responses.Organisations
{
    public class RetrieveOrganisationsResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<GetOrganisations> Organisations { get; set; }
    }

    public class GetOrganisations
    {
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationStatus { get; set; }
        public string LastModified { get; set; }
    }
}
