using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUser, AddUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOrganisationRepository _userOrganisationRepository;

        public AddUserCommandHandler(IUserRepository userRepository, IUserOrganisationRepository userOrganisationRepository)
        {
            _userRepository = userRepository;
            _userOrganisationRepository = userOrganisationRepository;
        }

        public async Task<AddUserResponse> Handle(AddUser request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                ProviderId = request.ProviderId,
                Value = new UserValue()
                {
                    EmailAddress = request.Email,
                    Name = request.Name,
                    Surname = request.Surname
                }
            };

            var existingUser = await _userRepository.GetByProviderIdAsync(user.ProviderId);

            if (existingUser != null)
            {
                return new AddUserResponse()
                {
                    UserId = existingUser.Id.ToString()
                };
            }

            var createdUser = await _userRepository.AddAsync(user, Guid.Parse(user.ProviderId), cancellationToken);

            if (!String.IsNullOrEmpty(request.InvitationId))
            {
                var userOrganisation = await _userOrganisationRepository.GetByIdAsync(Guid.Parse(request.InvitationId));
                userOrganisation.UserId = createdUser.Id;
                await _userOrganisationRepository.UpdateAsync(userOrganisation, createdUser.Id, cancellationToken);
            }
            
            return new AddUserResponse()
            {
                UserId = createdUser.Id.ToString()
            };
        }
    }

    public class AddUser : IRequest<AddUserResponse>
    {
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string InvitationId { get; set; }
    }
    
    public class AddUserResponse
    {
        public string UserId { get; set; }
    }
}