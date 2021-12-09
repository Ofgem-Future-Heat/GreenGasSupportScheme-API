using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Persistence.Repositories;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class AddUserCommandHandlerTests
    {
        // TODO: ***Upon User registration*** (i.e. not here - in the AddUserCommandHandler)
            //When the user registers, if the state parameter contains a UserOrganisation association ID
            //Set the user Id on that UserOrganisation with the new user Id (if it's not already set)
        
        [Fact]
        public async Task AddsUserToDatabase()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUserOrganisationRepository = new Mock<IUserOrganisationRepository>();
            var handler = new AddUserCommandHandler(mockUserRepository.Object, mockUserOrganisationRepository.Object);
            
            var addUserRequest = new AddUser()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Name = "Bob",
                Surname = "Bobson",
                Email = "bob@thebobsons.com"
            };

            var user = new User()
            {
                Id = new Guid()
            };
            
            mockUserRepository.Setup(r => r.AddAsync(It.Is<User>(u => u.ProviderId == addUserRequest.ProviderId),
                It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            
            var response = await handler.Handle(addUserRequest, CancellationToken.None);

            response.UserId.Should().Be(user.Id.ToString());
        }
        
        [Fact]
        public async Task AddsInvitedUserToDatabase()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUserOrganisationRepository = new Mock<IUserOrganisationRepository>();
            var handler = new AddUserCommandHandler(mockUserRepository.Object, mockUserOrganisationRepository.Object);

            var userOrganisation = new UserOrganisation()
            {
                Id = Guid.NewGuid(),
                OrganisationId = Guid.NewGuid(),
                UserId = null,
            };
            
            var addUserRequest = new AddUser()
            {
                ProviderId = Guid.NewGuid().ToString(),
                InvitationId = userOrganisation.Id.ToString(),
                Name = "Bob",
                Surname = "Bobson",
                Email = "bob@thebobsons.com"
            };
            
            var user = new User()
            {
                Id = Guid.NewGuid()
            };
            
            mockUserRepository.Setup(r => r.AddAsync(It.Is<User>(u => u.ProviderId == addUserRequest.ProviderId),
                It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            mockUserOrganisationRepository
                .Setup(r => r.GetByIdAsync(userOrganisation.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userOrganisation);

            
            await handler.Handle(addUserRequest, CancellationToken.None);

            mockUserOrganisationRepository.Verify(r =>
                r.UpdateAsync(It.Is<UserOrganisation>(e => e.Id == userOrganisation.Id && e.UserId == user.Id),
                    It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        }
    }
}