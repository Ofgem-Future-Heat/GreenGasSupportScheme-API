using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.Queries.Organisations;
using Ofgem.API.GGSS.WebApi.Attributes;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ofgem.API.GGSS.Domain.Commands.Applications;

namespace Ofgem.API.GGSS.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganisationsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OrganisationsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("getAllOrganisations")]
        public async Task<IActionResult> GetAllOrganisations()
        {
            var request = new RetrieveOrganisations();

            var response = await _mediator.Send(request);

            if (response.Errors.Contains("ORGANISATION_NOT_FOUND"))
            {
                return NotFound();
            }
            
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetForUserAsync([FromQuery(Name = "includeDocuments")] bool includeDocuments, CancellationToken cancellationToken)
        {
            var providerId = Request.HttpContext.User?.Claims?.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var query = new OrganisationsListForUser { ProviderId = providerId };
            var data = await _mediator.Send(query, cancellationToken);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("AddWithResponsiblePerson", Name = "SaveOrganisationWithResponsiblePerson")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddWithResponsiblePersonAsync([FromBody] OrganisationSave organisationSave, CancellationToken cancellationToken)
        {
            var request = new OrganisationSave()
            {
                UserId = organisationSave.UserId,
                Model = organisationSave.Model
            };

            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }
    }
}
