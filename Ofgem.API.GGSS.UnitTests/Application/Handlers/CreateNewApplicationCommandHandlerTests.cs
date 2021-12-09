using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class CreateNewApplicationCommandHandlerTests
    {
        private Mock<IApplicationRepository> _repository;
        private Guid _applicationId;
        private Guid _organisationId;
        private CreateNewApplicationResponse _result;

        [Fact]
        public async Task HandlerCreatesNewApplicationRecordInDbBelongingToCorrectOrganisation()
        {
            await ExecuteTheHandler();
            EnsureApplicationIsSavedWithCorrectOrganisationId();
        }

        [Fact]
        public async Task HandlerReturnsIdOfNewlyCreatedApplication()
        {
            await ExecuteTheHandler();
            EnsureHandlerRespondsWithNewlyCreatedApplicationId();
        }

        private void EnsureHandlerRespondsWithNewlyCreatedApplicationId()
        {
            _result.NewApplicationId.Should().Be(_applicationId.ToString());
        }
        
        private void EnsureApplicationIsSavedWithCorrectOrganisationId()
        {
            _repository.Verify(r
                => r.AddAsync(
                    It.Is<GGSS.Application.Entities.Application>(e => e.OrganisationId == _organisationId),
                    It.IsAny<Guid>(),
                    CancellationToken.None
                ), Times.Once);
        }

        private async Task ExecuteTheHandler()
        {
            _applicationId = Guid.NewGuid();
            _organisationId = Guid.NewGuid();
            
            _repository = new Mock<IApplicationRepository>();
            var handler = new CreateNewApplicationCommandHandler(_repository.Object);

            var application = new CreateNewApplication
            {
                UserId = Guid.NewGuid().ToString(),
                OrganisationId = _organisationId.ToString()
            };


            var applicationEntity = new GGSS.Application.Entities.Application() {Id = _applicationId};

            _repository.Setup(r
                => r.AddAsync(
                    It.IsAny<GGSS.Application.Entities.Application>(),
                    It.IsAny<Guid>(),
                    CancellationToken.None
                )).Returns(Task.FromResult(applicationEntity));
            
            _result = await handler.Handle(application, CancellationToken.None);
        }
    }
}