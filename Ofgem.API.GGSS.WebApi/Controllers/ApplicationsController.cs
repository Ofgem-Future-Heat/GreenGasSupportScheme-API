using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Queries.Applications;
using Ofgem.API.GGSS.WebApi.Attributes;

namespace Ofgem.API.GGSS.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ApplicationsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("StageOne", Name = "SaveStageOne")]
        public async Task<IActionResult> PostStageOneAsync([FromBody] StageOne stageOne, CancellationToken cancellationToken)
        {
            var request = new ApplicationSave<StageOne>(Request.HttpContext.User) { Model = stageOne };
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("GetAllApplications", Name = "GetAllApplications")]
        public async Task<IActionResult> GetAllApplications(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new RetrieveApplications(), cancellationToken);

            if (response.Errors.Any())
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
