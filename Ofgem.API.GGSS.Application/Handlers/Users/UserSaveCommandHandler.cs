using AutoMapper;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Commands.Users;
using Ofgem.API.GGSS.Domain.Responses.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofgem.API.GGSS.Application.Handlers.Users
{
    public class UserSaveCommandHandler : IRequestHandler<UserSave<UserRegistration>, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserSaveCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<bool> Handle(UserSave<UserRegistration> request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.Model);
            user.ProviderId = request.Model.ProviderId;

            var taskAsync = await _userRepository.AddAsync(user, Guid.Parse(request.Model.ProviderId), token: cancellationToken)
                .ContinueWith(async saveAsync =>
                {
                    var newUser = await saveAsync;
                    return await _userRepository.UserIsRegisteredAsync(newUser.Id, cancellationToken);
                }, 
                cancellationToken);

            return await taskAsync;
        }
    }
}
