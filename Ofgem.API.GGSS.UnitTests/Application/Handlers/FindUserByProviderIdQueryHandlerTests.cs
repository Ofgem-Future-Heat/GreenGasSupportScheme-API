using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class FindUserByProviderIdQueryHandlerTests
    {
        [Fact]
        public async Task ReturnsNotFoundWhenUserDoesNotExist()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            
            var handler = new FindUserByProviderIdQueryHandler(mockUserRepository.Object);
            var result = await handler.Handle(new FindUserByProviderId()
            {
                ProviderId = "notarealuser"
            }, CancellationToken.None);

            result.Errors.Should().Contain("USER_NOT_FOUND");
        }
        
        [Fact]
        public async Task ReturnsUserIdWhenUserDoesExist()
        {
            var mockUserRepository = new Mock<IUserRepository>();

            var providerId = Guid.NewGuid().ToString();

            var user = new User()
            {
                Id = Guid.NewGuid()
            };
            
            mockUserRepository.Setup(r => r.GetByProviderIdAsync(providerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            
            var handler = new FindUserByProviderIdQueryHandler(mockUserRepository.Object);
            var result = await handler.Handle(new FindUserByProviderId()
            {
                ProviderId = providerId
            }, CancellationToken.None);

            result.UserId.Should().Be(user.Id.ToString());
        }
    }
}