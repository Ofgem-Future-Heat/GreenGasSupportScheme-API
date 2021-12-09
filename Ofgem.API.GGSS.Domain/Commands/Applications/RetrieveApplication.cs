using System;
using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Domain.Commands.Applications
{
    public class RetrieveApplication : IRequest<RetrieveApplicationResponse>
    {
        public string ApplicationId { get; set; }
    }
}