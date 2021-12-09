using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.ModelValues;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class InviteUserToOrganisationCommandHandlerTests
    {
        [Fact]
        public async Task LinkUserToOrganisationWithMatchingEmailAddress()
        {
            var userRepository = new Mock<IUserRepository>();
            var userOrganisationRepository = new Mock<IUserOrganisationRepository>();

            var userId = Guid.NewGuid();
            var organisationId = Guid.NewGuid();

            userRepository.Setup(u => u.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>()
                {
                    new User()
                    {
                        Id = userId,
                        Value = new UserValue()
                        {
                            EmailAddress = "match@test.com"
                        }
                    }
                });

            var handler = new InviteUserToOrganisationCommandHandler(userRepository.Object, userOrganisationRepository.Object);

            var request = new InviteUserToOrganisation()
            {
                OrganisationId = organisationId.ToString(),
                UserEmail = "match@test.com"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            userOrganisationRepository.Verify(u =>
                u.AddAsync(It.Is<UserOrganisation>(
                        e => e.UserId == userId && e.OrganisationId == organisationId), It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            
        }

        [Fact]
        public async Task CreateInviteForUserWhoHasNotRegistered()
        {
            var userRepository = new Mock<IUserRepository>();
            var userOrganisationRepository = new Mock<IUserOrganisationRepository>();

            var userId = Guid.NewGuid();
            var organisationId = Guid.NewGuid();
            var associationId = Guid.NewGuid();

            userRepository.Setup(u => u.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>()
                {
                    new User()
                    {
                        Id = userId,
                        Value = new UserValue()
                        {
                            EmailAddress = "unique@test.com"
                        }
                    }
                });

            userOrganisationRepository.Setup(u =>
                u.AddAsync(It.Is<UserOrganisation>(
                        e => e.UserId == null && e.OrganisationId == organisationId), It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new UserOrganisation()
            {
                Id = associationId
            });
            
            var handler = new InviteUserToOrganisationCommandHandler(userRepository.Object, userOrganisationRepository.Object);

            var request = new InviteUserToOrganisation()
            {
                OrganisationId = organisationId.ToString(),
                UserEmail = "match@test.com"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.InvitationId.Should().Be(associationId.ToString());
        }
    }
}