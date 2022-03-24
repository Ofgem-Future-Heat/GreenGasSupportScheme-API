using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Responses.Organisations;

namespace Ofgem.API.GGSS.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganisationController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OrganisationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpGet("{OrganisationId}")]
        public async Task<IActionResult> RetrieveOrganisationById(string organisationId)
        {
            var request = new RetrieveOrganisation()
            {
                OrganisationId = organisationId
            };
            var response = await _mediator.Send(request);
            
            if (response.Errors.Contains("ORGANISATION_NOT_FOUND")) 
            {
                return NotFound();
            }
            
            return Ok(response);
        }

        [HttpGet("{organisationId}/details")]
        public async Task<IActionResult> RetrieveOrganisationDetails(string organisationId)
        {
            var request = new RetrieveOrganisationDetails()
            {
                OrganisationId = organisationId,
                UserId = Request.Query["userId"]
            };

            var response = await _mediator.Send(request);

            if (response.Errors.Contains("ORGANISATION_DETAILS_NOT_FOUND"))
            {
                return NotFound();
            }

            return Ok(response);
        }
        
        [HttpPatch("{organisationId}/details")]
        public async Task<IActionResult> UpdateOrganisationDetails(string organisationId, [FromBody] UpdateOrganisationRequest request)
        {
            var response = await _mediator.Send(new UpdateOrganisation()
            {
                OrganisationId = organisationId,
                OrganisationStatus = request.OrganisationStatus,
                UserId = request.UserId
            });

            if (response.Errors.Contains("ORGANISATION_NOT_FOUND"))
            {
                return NotFound();
            }

            return Ok(response);
        }

        public class UpdateOrganisationRequest
        {
            public string OrganisationStatus { get; set; }
            public string UserId { get; set; }
        }
        
    }
}