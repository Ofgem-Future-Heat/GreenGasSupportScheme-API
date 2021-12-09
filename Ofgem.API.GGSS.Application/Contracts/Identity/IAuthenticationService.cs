using Ofgem.API.GGSS.Application.Models.Authentication;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    }
}
