using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Responses.Applications
{
    public class RetrieveApplicationsResponse
    {
        public List<GetApplications> List { get; set; }

        public List<string> Errors { get; internal set; } = new List<string>();
    }

    public class GetApplications
    {
        public string ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string ApplicationStatus { get; set; }
        public string LastModified { get; set; }
        public string Reference { get; set; }
    }
}
