using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using System;
using System.Linq;
using System.Security.Claims;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class ApplicationSave<TStage> : IRequest<string>, IRequest<ApplicationResponse> where TStage : class
    {
        public string UserId { get; private set; }
        public TStage Model { get; set; }
        public ApplicationSave(){}
        public ApplicationSave(ClaimsPrincipal user) : this()
        {
            if(user == null) throw new ArgumentNullException(nameof(user));
            this.UserId = user.Claims?.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
