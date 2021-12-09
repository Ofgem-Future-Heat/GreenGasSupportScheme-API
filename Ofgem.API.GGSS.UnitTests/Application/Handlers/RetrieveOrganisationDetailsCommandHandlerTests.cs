using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class RetrieveOrganisationDetailsCommandHandlerTests
    {
        [Fact]
        public async Task ReturnOrganisationDetails()
        {
            var OrganisationId = "1234";
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetOrganisationDetailsByOrgId(OrganisationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Organisation()
                {
                    Value = new OrganisationValue()
                    {
                        Name = "Organisation Name",
                        Type = OrganisationType.Other,
                        RegistrationNumber = "5678"
                    }
                });

            var handler = new RetrieveOrganisationDetailsCommandHandler(repository.Object);
            var request = new RetrieveOrganisationDetails() {OrganisationId = OrganisationId};

            var result = await handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.OrganisationName.Should().Be("Organisation Name");
            result.OrganisationRegistrationNumber.Should().Be("5678");
        }
    }
}