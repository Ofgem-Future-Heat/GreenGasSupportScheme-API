using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Application.Handlers.Users;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Commands.Users;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class GetOrganisationsForUserQueryHandlerTests
    {
        [Fact]
        public async Task GetsAnOrganisationsWhenGivenAUser()
        {
            var repository = new Mock<IOrganisationRepository>();

            repository.Setup(a => a.ListAllForUserAsync("UserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>());

            var handler = new GetOrganisationsForUserQueryHandler(repository.Object);

            var request = new GetOrganisationsForUser()
            {
                UserId = "UserId"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GetListOfOrganisationsForNonExistentUser()
        {
            var repository = new Mock<IOrganisationRepository>();
            var handler = new GetOrganisationsForUserQueryHandler(repository.Object);
            var userId = new Guid();

            var request = new GetOrganisationsForUser()
            {
                UserId = userId.ToString()
            };

            var result = await handler.Handle(request, CancellationToken.None);
            
            repository.Verify(a => a.ListAllForUserAsync(userId.ToString(), CancellationToken.None), Times.Once);

            result.Errors.Should().Contain("USER_NOT_FOUND");
        }
        
        [Fact]
        public async Task ReturnsOrganisationIdsForUser()
        {
            var repository = new Mock<IOrganisationRepository>();
            var organisationId = new Guid();

            repository.Setup(a => a.ListAllForUserAsync("UserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>()
                {
                    new Organisation()
                    {
                        Id = organisationId,
                        Value = new OrganisationValue()
                    }
                });

            var handler = new GetOrganisationsForUserQueryHandler(repository.Object);

            var request = new GetOrganisationsForUser()
            {
                UserId = "UserId"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should().Contain(organisation => organisation.OrganisationId == organisationId.ToString());
        }
        
        [Fact]
        public async Task ReturnsOrganisationNamesForUser()
        {
            var repository = new Mock<IOrganisationRepository>();
            var organisationName = "Org name";

            repository.Setup(a => a.ListAllForUserAsync("UserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>()
                {
                    new Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = organisationName
                        }
                    }
                });

            var handler = new GetOrganisationsForUserQueryHandler(repository.Object);

            var request = new GetOrganisationsForUser()
            {
                UserId = "UserId"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should().Contain(organisation => organisation.OrganisationName == organisationName);
        }
        
        [Fact]
        public async Task ReturnsOrganisationApplicationCountsForUser()
        {
            var repository = new Mock<IOrganisationRepository>();
            var organisationName = "Org name";

            repository.Setup(a => a.ListAllForUserAsync("UserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>()
                {
                    new Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = organisationName
                        },
                        Applications = new List<GGSS.Application.Entities.Application>()
                        {
                            new GGSS.Application.Entities.Application(),
                            new GGSS.Application.Entities.Application(),
                            new GGSS.Application.Entities.Application()
                        }
                    }
                });

            var handler = new GetOrganisationsForUserQueryHandler(repository.Object);

            var request = new GetOrganisationsForUser()
            {
                UserId = "UserId"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should().Contain(organisation => organisation.NumberOfApplications == 3);
        }
    }
}
