using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class RetrieveApplications : IRequest<RetrieveApplicationsResponse> { }
}
