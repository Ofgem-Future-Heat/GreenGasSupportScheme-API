using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.ModelValues.StageTwo;
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
        private Guid _adminUserId;
        private Guid _responsiblePersonId;

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

        [Fact]
        public async Task ReturnsNonSubmittableApplicationForAdminUser()
        {
            SetUpTheRepositoryWithAnApplication();
            await RetrieveAnExistingApplicationAsAdminUser();

            _result.Application.CanSubmit.Should().BeFalse();
        }
        
        [Fact]
        public async Task ReturnsSubmittableApplicationForResponsiblePerson()
        {
            SetUpTheRepositoryWithAnApplication();
            await RetrieveAnExistingApplicationAsResponsiblePerson();

            _result.Application.CanSubmit.Should().BeTrue();
        }
        
        [Fact]
        public async Task UpdatesRejectedStatusToBeStageOneRejected()
        {
            SetUpTheRepositoryWithAnApplication();
            await RetrieveAnExistingApplication();
            
            _result.Application.Status.Should().Be(ApplicationStatus.StageOneRejected);
        }
        
        [Fact]
        public async Task UpdatesRejectedStatusToBeStageTwoRejected()
        {
            SetUpTheRepositoryWithAStageTwoApplication();
            await RetrieveAnExistingApplication();
            
            _result.Application.Status.Should().Be(ApplicationStatus.StageTwoRejected);
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
        
        private async Task RetrieveAnExistingApplicationAsAdminUser()
        {
            var request = new RetrieveApplication()
            {
                ApplicationId = _applicationId.ToString(),
                UserId = _adminUserId.ToString()
            };

            var handler = new RetrieveApplicationCommandHandler(_repository.Object);

            _result = await handler.Handle(request, CancellationToken.None);
        }
        
        private async Task RetrieveAnExistingApplicationAsResponsiblePerson()
        {
            var request = new RetrieveApplication()
            {
                ApplicationId = _applicationId.ToString(),
                UserId = _responsiblePersonId.ToString()
            };

            var handler = new RetrieveApplicationCommandHandler(_repository.Object);

            _result = await handler.Handle(request, CancellationToken.None);
        }

        private void SetUpTheRepositoryWithAnApplication()
        {
            _repository = new Mock<IApplicationRepository>();
            _responsiblePersonId = Guid.NewGuid();
            _adminUserId = Guid.NewGuid();

            _applicationInDb = new GGSS.Application.Entities.Application()
            {
                Value = new ApplicationValue()
                {
                    Status = ApplicationStatus.Rejected,
                    StageOne = new StageOneValue()
                    {
                        TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                        {
                            PlantName = "Some Plant"
                        }
                    }
                },
                Organisation = new Organisation
                {
                    ResponsiblePeople = new List<ResponsiblePerson>()
                    {
                        new ResponsiblePerson()
                        {
                            UserId = _responsiblePersonId
                        }
                    }
                }
            };

            _applicationId = Guid.NewGuid();
            _repository.Setup(r => r.GetById(_applicationId, CancellationToken.None))
                .Returns(Task.FromResult(_applicationInDb));
        }
        
        private void SetUpTheRepositoryWithAStageTwoApplication()
        {
            _repository = new Mock<IApplicationRepository>();
            _responsiblePersonId = Guid.NewGuid();
            _adminUserId = Guid.NewGuid();

            _applicationInDb = new GGSS.Application.Entities.Application()
            {
                Value = new ApplicationValue()
                {
                    Status = ApplicationStatus.Rejected,
                    StageOne = new StageOneValue()
                    {
                        TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                        {
                            PlantName = "Some Plant"
                        }
                    },
                    StageTwo = new StageTwoValue()
                    {
                        Isae3000 = new Isae3000Value()
                        {
                            Status = "Submitted"
                        }
                    }
                },
                Organisation = new Organisation
                {
                    ResponsiblePeople = new List<ResponsiblePerson>()
                    {
                        new ResponsiblePerson()
                        {
                            UserId = _responsiblePersonId
                        }
                    }
                }
            };

            _applicationId = Guid.NewGuid();
            _repository.Setup(r => r.GetById(_applicationId, CancellationToken.None))
                .Returns(Task.FromResult(_applicationInDb));
        }
    }
}