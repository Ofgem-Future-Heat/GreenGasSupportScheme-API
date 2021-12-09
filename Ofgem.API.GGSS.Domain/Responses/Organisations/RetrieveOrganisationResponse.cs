using System;
using System.Collections.Generic;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Responses.Organisations
{
    public class RetrieveOrganisationResponse
    {
        public string Name { get; set; }
        public List<RetrieveOrganisationApplicationResponse> Applications { get; set; }
        public List<String> Errors { get; set; } = new List<string>();
    }

    public class RetrieveOrganisationApplicationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}