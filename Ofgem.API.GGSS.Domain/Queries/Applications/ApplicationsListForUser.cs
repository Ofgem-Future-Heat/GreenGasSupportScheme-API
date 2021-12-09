using MediatR;
using Ofgem.API.GGSS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ofgem.API.GGSS.Domain.Queries.Applications
{
    public class ApplicationsListForUser : IRequest<List<ApplicationModel>>
    {
        public string UserProviderId { get; set; }
        public bool IncludeDocuments { get; set; }
        public ApplicationsListForUser(){}
        public ApplicationsListForUser(ClaimsPrincipal user) : this()
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            this.UserProviderId = user.Claims?.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
