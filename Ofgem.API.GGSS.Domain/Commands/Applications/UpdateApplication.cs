using MediatR;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class UpdateApplication : IRequest<UpdateApplicationResponse>
    {
        public string ApplicationId { get; set; }
        public ApplicationValue Application { get; set; }
        
        public string UserId { get; set; }
    }
}