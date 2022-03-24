using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.WebApi.Attributes;

namespace Ofgem.API.GGSS.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public ApplicationController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewApplication([FromBody] CreateNewApplicationRequestModel createNewApplicationRequest)
        {
            var ggsUser = await _userRepository.GetByProviderIdAsync(createNewApplicationRequest.UserId);
            var ggsUserId = ggsUser != null ? ggsUser.Id.ToString() : createNewApplicationRequest.UserId;

            var command = new CreateNewApplication
            {
                UserId = ggsUserId, 
                OrganisationId = createNewApplicationRequest.OrganisationId
            };
            
            var response = await _mediator.Send(command);

            return Ok(response);
        }
        
        [HttpGet("{applicationId}")]
        public async Task<IActionResult> RetrieveApplicationById(string applicationId)
        {
            var command = new RetrieveApplication()
            {
                ApplicationId = applicationId,
                UserId = Request.Query["userId"]
            };

            var response = await _mediator.Send(command);

            if (response.Errors.Contains("APPLICATION_NOT_FOUND"))
            {
                return NotFound();
            }
            
            return Ok(response);
        }

        [HttpPut("{applicationId}")]
        public async Task<IActionResult> UpdateApplication(string applicationId, [FromBody] UpdateApplicationRequestModel updateApplicationRequest)
        {
            var ggsUser = await _userRepository.GetByProviderIdAsync(updateApplicationRequest.UserId);
            var ggsUserId = ggsUser != null ? ggsUser.Id.ToString() : updateApplicationRequest.UserId;

            var command = new UpdateApplication()
            {
                ApplicationId = applicationId,
                Application = updateApplicationRequest.Application,

                UserId = ggsUserId
            };

            var response = await _mediator.Send(command);

            if (response.Errors.Contains("APPLICATION_NOT_FOUND"))
            {
                return NotFound();
            }

            return Ok();
        }
    }
}