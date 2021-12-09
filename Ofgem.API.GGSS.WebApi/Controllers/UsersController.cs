using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ofgem.API.GGSS.Domain.Commands.Users;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Organisations;

namespace Ofgem.API.GGSS.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUser request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        
        [HttpGet("{userId}/organisations")]
        public async Task<IActionResult> GetOrganisationsByUser(string userId)
        {
            var request = new GetOrganisationsForUser
            {
                UserId = userId
            };

            var response = await _mediator.Send(request);
            
            if (response.Errors.Contains("ORGANISATIONS_NOT_FOUND")) 
            {
                return NotFound();
            }
            
            return Ok(response);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindUser()
        {
            var providerId = Request.Query["ProviderId"];

            var response = await _mediator.Send(new FindUserByProviderId()
            {
                ProviderId = providerId
            });

            if (response.Errors.Any())
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
