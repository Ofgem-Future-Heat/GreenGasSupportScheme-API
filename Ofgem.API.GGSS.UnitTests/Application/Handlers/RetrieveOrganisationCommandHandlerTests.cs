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
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class RetrieveOrganisationCommandHandlerTests
    {
        private Guid _organisationId;
        private RetrieveOrganisationResponse _result;
        private Mock<IOrganisationRepository> _database;
        private Organisation _organisation;

        [Fact]
        public async Task GetsAListOfApplicationsWhenGivenAnOrganisationId()
        {
            await CallTheHandler();

            _result.Applications.Should().NotBeNull();
        }
        
        [Fact]
        public async Task RequestsListOfApplicationsFromDatabaseForGivenOrganisationId()
        {
            await CallTheHandler();

            _database.Verify(a => a.GetByIdWithApplications(_organisationId.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task DatabaseReturnsCorrectApplicationsList()
        {
            await CallTheHandler();

            _result.Applications.First().Id.Should().Be(_organisation.Applications.First().Id.ToString());
        }

        [Fact]
        public async Task ThrowsErrorWhenOrganisationNotFound()
        {
            await CallTheHandlerWithOrganisationThatDoesntExist();
            _result.Errors.Should().Contain("ORGANISATION_NOT_FOUND");

        }
        private async Task CallTheHandler()
        {
            _database = new Mock<IOrganisationRepository>();
            _organisation = new Organisation()
            {
                Value = new OrganisationValue(),
                
                Applications = new List<GGSS.Application.Entities.Application>()
                {
                    new GGSS.Application.Entities.Application()
                    {
                        Id = Guid.NewGuid(),
                        Value = new ApplicationValue()
                        { 
                            Status = ApplicationStatus.Draft,
                            StageOne = new StageOneValue()
                            {
                                TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                            }
                        }
                    }
                }
            };
            _database.Setup(a => a.GetByIdWithApplications(_organisationId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(_organisation);
            var handler = new RetrieveOrganisationCommandHandler(_database.Object);
            var request = new RetrieveOrganisation()
            {
                OrganisationId = _organisationId.ToString()
            };

            _result = await handler.Handle(request, CancellationToken.None);
        }
        
        private async Task CallTheHandlerWithOrganisationThatDoesntExist()
        {
            _database = new Mock<IOrganisationRepository>();
            _database.Setup(a => a.GetByIdAsync(_organisationId, It.IsAny<CancellationToken>())).ReturnsAsync((Organisation)null);
            var handler = new RetrieveOrganisationCommandHandler(_database.Object);
            var request = new RetrieveOrganisation()
            {
                OrganisationId = _organisationId.ToString()
            };

            _result = await handler.Handle(request, CancellationToken.None);
        }
    }
}
