using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace Ofgem.API.GGSS.Application.Handlers
{
    public class FindUserByProviderIdQueryHandler : IRequestHandler<FindUserByProviderId, FindUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public FindUserByProviderIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<FindUserResponse> Handle(FindUserByProviderId request, CancellationToken cancellationToken)
        {
            var response = new FindUserResponse();

            var user = await _userRepository.GetByProviderIdAsync(request.ProviderId, cancellationToken);

            if (user == null)
            {
                response.Errors.Add("USER_NOT_FOUND");
                return response;
            }

            response.UserId = user.Id.ToString();

            return response;
        }
    }

    public class FindUserByProviderId : IRequest<FindUserResponse>
    {
        public string ProviderId { get; set; }
    }
    public class FindUserResponse 
    {
        public string UserId { get; set; }
        public List<String> Errors { get; set; } = new List<string>();
    }
}