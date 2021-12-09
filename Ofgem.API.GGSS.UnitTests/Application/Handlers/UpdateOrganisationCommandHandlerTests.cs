using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.ModelValues;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class UpdateOrganisationCommandHandlerTests
    {
        private Mock<IOrganisationRepository> _repository;

        [Fact]
        public async Task HandlerUpdatesOrganisationStatus()
        {
            _repository = new Mock<IOrganisationRepository>();
            
            var handler = new UpdateOrganisationCommandHandler(_repository.Object);

            var orgId = Guid.NewGuid();
            
            var command = new UpdateOrganisation()
            {
                OrganisationId = orgId.ToString(),
                OrganisationStatus = "Verified"
            };

            _repository.Setup(r => r.GetByIdAsync(orgId, It.IsAny<CancellationToken>())).ReturnsAsync(new Organisation()
            {
                Id = orgId,
                Value = new OrganisationValue()
                {
                    OrganisationStatus = "Not Verified"
                }
            });
            
            await handler.Handle(command, CancellationToken.None);

            _repository.Verify(
                r => r.UpdateAsync(
                    It.Is<Organisation>(o =>
                        o.Id == orgId && o.Value.OrganisationStatus ==
                        command.OrganisationStatus), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task HandlerReturnsErrorIfOrgNotFound()
        {
            _repository = new Mock<IOrganisationRepository>();
            
            var handler = new UpdateOrganisationCommandHandler(_repository.Object);

            var orgId = Guid.NewGuid();
            
            var command = new UpdateOrganisation()
            {
                OrganisationId = orgId.ToString(),
                OrganisationStatus = "Verified"
            };

            _repository.Setup(r => r.GetByIdAsync(orgId, It.IsAny<CancellationToken>())).ReturnsAsync((Organisation)null);
            
            var result =await handler.Handle(command, CancellationToken.None);

            result.Errors.Should().Contain("ORGANISATION_NOT_FOUND");
        }
    }
}