using Ofgem.API.GGSS.Application.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ofgem.API.GGSS.WebApi.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}
