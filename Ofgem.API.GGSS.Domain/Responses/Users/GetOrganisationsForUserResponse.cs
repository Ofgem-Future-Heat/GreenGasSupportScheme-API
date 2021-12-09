using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.Domain.Responses.Users
{
    public class GetOrganisationsForUserResponse
    {
        public List<String> Errors { get; set; } = new List<string>();
        public List<GetOrganisationForUser> Organisations { get; set; }
    }

    public class GetOrganisationForUser
    {
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int NumberOfApplications { get; set; }
    }
}