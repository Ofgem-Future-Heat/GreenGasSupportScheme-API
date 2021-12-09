using System;
using System.Collections.Generic;
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
using Ofgem.API.GGSS.Domain.ModelValues;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class RetrieveOrganisationsCommandHandlerTests
    {
        [Fact]
        public async Task GetListOfAllOrganisations()
        {
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetAllOrganisations(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>());

            var handler = new RetrieveOrganisationsCommandHandler(repository.Object);

            var request = new RetrieveOrganisations();

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should().NotBeNull();
        }

        [Fact]
        public async Task ReturnsOrganisationsNames()
        {
            var OrganisationNameOne = "OrgNameOne";
            var OrganisationNameTwo = "OrgNameTwo";
            
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetAllOrganisations(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>()
                {
                    new Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = OrganisationNameOne
                        }
                    },
                    new Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = OrganisationNameTwo
                        }
                    }
                });

            var handler = new RetrieveOrganisationsCommandHandler(repository.Object);

            var request = new RetrieveOrganisations();

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should()
                .Contain(organisation => organisation.OrganisationName == OrganisationNameTwo);
        }

        [Fact]
        public async Task ReturnsOrganisationsIds()
        {
            var organisationIdOne = new Guid();
            var organisationNameOne = "OrgNameOne";

            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetAllOrganisations(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Organisation>()
                {
                    new Organisation()
                    {
                        Id = organisationIdOne,
                        Value = new OrganisationValue()
                        {
                            Name = organisationNameOne
                        }
                    }
                });
            
            var handler = new RetrieveOrganisationsCommandHandler(repository.Object);

            var request = new RetrieveOrganisations();

            var result = await handler.Handle(request, CancellationToken.None);

            result.Organisations.Should()
                .Contain(organisation => organisation.OrganisationId == organisationIdOne.ToString());
        }
    }
}