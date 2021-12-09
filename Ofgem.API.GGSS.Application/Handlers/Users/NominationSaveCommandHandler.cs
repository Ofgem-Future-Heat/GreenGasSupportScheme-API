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
    public class NominationSaveCommandHandler : IRequestHandler<UserSave<UserRegistration>, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public NominationSaveCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<UserResponse> Handle(UserSave<UserRegistration> request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.Model);

            var taskAsync = await _userRepository.AddAsync(user, Guid.Parse(request.Model.ProviderId), token: cancellationToken)
                .ContinueWith(async saveAsync =>
                {
                    var newUser = await saveAsync;
                    return _mapper.Map<UserResponse>(newUser);
                }, 
                cancellationToken);

            return await taskAsync;
        }
    }
}
