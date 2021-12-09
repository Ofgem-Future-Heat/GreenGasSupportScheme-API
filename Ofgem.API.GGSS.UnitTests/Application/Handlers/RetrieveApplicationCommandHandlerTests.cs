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
    public class RetrieveApplicationCommandHandlerTests
    {
        private Mock<IApplicationRepository> _repository;
        private Guid _applicationId;
        private GGSS.Application.Entities.Application _applicationInDb;
        private RetrieveApplicationResponse _result;

        [Fact]
        public async Task RetrievesAnApplicationFromTheDatabase()
        {
            SetUpTheRepositoryWithAnApplication();
            await RetrieveAnExistingApplication();
            EnsureApplicationWasRetrieved();
        }
        
        [Fact]
        public async Task ReturnsNotFoundErrorIfApplicationNotFound()
        {
            SetUpTheRepositoryWithAnApplication();
            await TryToRetrieveAnApplicationThatDoesntExist();
            EnsureHandlerRespondsWithApplicationNotFoundError();
        }

        private void EnsureHandlerRespondsWithApplicationNotFoundError()
        {
            _result.Errors.Should().Contain("APPLICATION_NOT_FOUND");
        }
        
        private void EnsureApplicationWasRetrieved()
        {
            _result.Errors.Should().BeEmpty();
            _result.Application.StageOne.TellUsAboutYourSite.PlantName.Should()
                .Be(_applicationInDb.Value.StageOne.TellUsAboutYourSite.PlantName);
        }

        private async Task TryToRetrieveAnApplicationThatDoesntExist()
        {
            var handler = new RetrieveApplicationCommandHandler(_repository.Object);

            var request = new RetrieveApplication()
            {
                ApplicationId = Guid.Empty.ToString()
            };

            _result = await handler.Handle(request, CancellationToken.None);
        }

        private async Task RetrieveAnExistingApplication()
        {
            var request = new RetrieveApplication()
            {
                ApplicationId = _applicationId.ToString()
            };

            var handler = new RetrieveApplicationCommandHandler(_repository.Object);

            _result = await handler.Handle(request, CancellationToken.None);
        }

        private void SetUpTheRepositoryWithAnApplication()
        {
            _repository = new Mock<IApplicationRepository>();

            _applicationInDb = new GGSS.Application.Entities.Application()
            {
                Value = new ApplicationValue()
                {
                    StageOne = new StageOneValue()
                    {
                        TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                        {
                            PlantName = "Some Plant"
                        }
                    }
                }
            };

            _applicationId = Guid.NewGuid();
            _repository.Setup(r => r.GetByIdAsync(_applicationId, CancellationToken.None))
                .Returns(Task.FromResult(_applicationInDb));
        }
    }
}