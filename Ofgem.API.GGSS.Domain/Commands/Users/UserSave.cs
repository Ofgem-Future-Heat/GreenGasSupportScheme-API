using MediatR;
using Ofgem.API.GGSS.Domain.Responses.Users;
using System;
using System.Linq;
using System.Security.Claims;

namespace Ofgem.API.GGSS.Domain.Commands.Users
{
    public class UserSave<TUser> : IRequest<bool>, IRequest<UserResponse> where TUser : class
    {
        public string UserId { get; set; }
        public TUser Model { get; set; }
        public UserSave(){}

        public UserSave(TUser user) : this()
        {
            this.Model = user ?? throw new ArgumentNullException(nameof(user));

        }
        public UserSave(TUser user, ClaimsPrincipal currentUser) : this(user)
        {
            this.UserId = currentUser.Claims?.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
