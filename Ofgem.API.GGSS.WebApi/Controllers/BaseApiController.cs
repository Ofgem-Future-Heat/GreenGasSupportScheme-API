using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc
{
    public class BaseApiController : ControllerBase
    {
        public BaseApiController() { }

        protected static readonly IReadOnlyCollection<string> ClaimsToSendBack = new HashSet<string>
        {
            "iat", "nbf", "exp", "appid", "appidacr", "e_exp",
            "https://schemas.microsoft.com/identity/claims/objectidentifier",
            "https://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            "https://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        };
    }
}
