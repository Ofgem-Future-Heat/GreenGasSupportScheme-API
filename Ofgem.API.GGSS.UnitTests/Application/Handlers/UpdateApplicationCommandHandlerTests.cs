using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class UpdateApplicationCommandHandlerTests
    {
        private Mock<IApplicationRepository> _repository;
        private Guid _applicationId;
        private Guid _organisationId;
        private UpdateApplicationResponse _result;
        private ApplicationValue _application;

        [Fact]
        public async Task HandlerUpdatesMatchingApplicationInDb()
        {
            SetUpRepositoryWithAnApplicationThatMatchesTheRequest();
            await ExecuteTheHandler();
            EnsureApplicationIsSavedWithUpdatedData();
        }

        [Fact]
        public async Task ReturnsNotFoundIfApplicationDoesntExist()
        {
            SetUpRepositoryWithoutMatchingApplication();
            await ExecuteTheHandler();
            EnsureHandlerRespondsWithApplicationNotFoundError();
        }

        
        
        private void EnsureApplicationIsSavedWithUpdatedData()
        {
            _repository.Verify(r
                => r.UpdateAsync(
                    It.Is<GGSS.Application.Entities.Application>(e => e.Value.StageOne.TellUsAboutYourSite.PlantName == _application.StageOne.TellUsAboutYourSite.PlantName),
                    It.IsAny<Guid>(),
                    CancellationToken.None
                ), Times.Once);
        }
        
        private void EnsureHandlerRespondsWithApplicationNotFoundError()
        {
            _result.Errors.Should().Contain("APPLICATION_NOT_FOUND");
        }

        private async Task ExecuteTheHandler()
        {
            var updateApplication = new UpdateApplication()
            {
                UserId = Guid.NewGuid().ToString(),
                ApplicationId = Guid.NewGuid().ToString(),
                Application = _application
            };
            
            var handler = new UpdateApplicationCommandHandler(_repository.Object);
            _result = await handler.Handle(updateApplication, CancellationToken.None);
        }

        private void SetUpRepositoryWithAnApplicationThatMatchesTheRequest()
        {
            _applicationId = Guid.NewGuid();
            _organisationId = Guid.NewGuid();

            _repository = new Mock<IApplicationRepository>();

            _application = new ApplicationValue()
            {
                StageOne = new StageOneValue()
                {
                    TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                    {
                        PlantName = $"My plant {Guid.NewGuid()}"
                    }
                }
            };

            var applicationEntity = new GGSS.Application.Entities.Application() {Id = _applicationId};

            _repository.Setup(r
                => r.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns(Task.FromResult(applicationEntity));

            _repository.Setup(r
                => r.UpdateAsync(
                    It.IsAny<GGSS.Application.Entities.Application>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns(Task.FromResult(applicationEntity));
        }
        
        private void SetUpRepositoryWithoutMatchingApplication()
        {
            _repository = new Mock<IApplicationRepository>();
            
            _repository.Setup(r
                => r.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                )
            ).Returns(Task.FromResult<GGSS.Application.Entities.Application>(null));
        }
    }
}