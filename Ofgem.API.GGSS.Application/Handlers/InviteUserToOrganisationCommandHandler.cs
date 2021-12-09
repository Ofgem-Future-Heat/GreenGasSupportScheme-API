using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class InviteUserToOrganisationCommandHandler : IRequestHandler<InviteUserToOrganisation, InviteUserToOrganisationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOrganisationRepository _userOrganisationRepository;

        public InviteUserToOrganisationCommandHandler(IUserRepository userRepository, IUserOrganisationRepository userOrganisationRepository)
        {
            _userRepository = userRepository;
            _userOrganisationRepository = userOrganisationRepository;
        }
        
        public async Task<InviteUserToOrganisationResponse> Handle(InviteUserToOrganisation request, CancellationToken cancellationToken)
        { 
            var result = await _userRepository.ListAllAsync(cancellationToken);

            var user = result.SingleOrDefault(u => u.Value.EmailAddress == request.UserEmail);
            
            if (user != null)
            {
                await _userOrganisationRepository.AddAsync(new UserOrganisation()
                {
                    Id = Guid.NewGuid(),
                    OrganisationId = Guid.Parse(request.OrganisationId),
                    UserId = user.Id
                }, token: cancellationToken);

                return new InviteUserToOrganisationResponse()
                {
                    InvitationResult = "USER_ADDED"
                };
            }

            var invitation = await _userOrganisationRepository.AddAsync(new UserOrganisation()
            {
                Id = Guid.NewGuid(),
                OrganisationId = Guid.Parse(request.OrganisationId),
            }, token: cancellationToken);

            return new InviteUserToOrganisationResponse()
            {
                InvitationId = invitation.Id.ToString(),
                InvitationResult = "USER_NEEDS_TO_REGISTER"
            };
        }
    }
    
    public class InviteUserToOrganisation : IRequest<InviteUserToOrganisationResponse>
    {
        public string OrganisationId { get; set; }
        public string UserEmail { get; set; }
    }
    
    public class InviteUserToOrganisationResponse
    {
        private List<string> Errors { get; set; } = new List<string>();
        
        public string InvitationResult { get; set; } // USER_ADDED || USER_REQUIRES_ACCOUNT

        public string InvitationId { get; set; }
    }
}