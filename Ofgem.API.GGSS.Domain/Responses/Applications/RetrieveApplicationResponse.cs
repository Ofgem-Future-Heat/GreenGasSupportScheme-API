using System;
using System.Collections.Generic;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Domain.Responses.Applications
{
    public class RetrieveApplicationResponse
    {
        public ApplicationValue Application { get; set; }
        
        public List<string> Errors { get; internal set;  } = new List<string>();

        public Guid OrganisationId { get; set; }
    }
}