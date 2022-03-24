using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.ModelValues.StageTwo;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class RetrieveApplicationsCommandHandlerTests
    {
        private Mock<IApplicationRepository> _repository;
        private IReadOnlyList<GGSS.Application.Entities.Application> _applicationsInDb => GetApplicationsInDb();

        public RetrieveApplicationsCommandHandlerTests()
        {
            _repository = new Mock<IApplicationRepository>();
        }

        [Fact]
        public async Task RetrievesStageOneApplicationsFromTheDatabase()
        {
            _repository
                .Setup(r => r.ListAll(CancellationToken.None))
                .Returns(Task.FromResult(_applicationsInDb));

            var request = new RetrieveApplications();

            var handler = new RetrieveApplicationsCommandHandler(_repository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Errors.Should().BeNullOrEmpty();
            result.List.Count.Should().Be(3);
        }

        [Fact]
        public async Task RetrievesStageOneApplicationsFromTheDatabaseReturnsErrorCollection()
        {
            _repository
                .Setup(r => r.ListAll(CancellationToken.None))
                .Returns(Task.FromResult<IReadOnlyList<GGSS.Application.Entities.Application>>(null));

            var request = new RetrieveApplications();

            var handler = new RetrieveApplicationsCommandHandler(_repository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Errors.Should().NotBeEmpty();
            result.List.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public async Task StageOneApplicationsWithRejectedStatusAreUpdatedToStageOneRejected()
        {
            _repository
                .Setup(r => r.ListAll(CancellationToken.None))
                .Returns(Task.FromResult(_applicationsInDb));

            var request = new RetrieveApplications();

            var handler = new RetrieveApplicationsCommandHandler(_repository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Errors.Should().BeNullOrEmpty();
            result.List.First().ApplicationStatus.Should().Be("StageOneRejected");
            result.List[1].ApplicationStatus.Should().Be("Draft");
        }
        
        [Fact]
        public async Task StageTwoApplicationsWithRejectedStatusAreUpdatedToStageTwoRejected()
        {
            _repository
                .Setup(r => r.ListAll(CancellationToken.None))
                .Returns(Task.FromResult(_applicationsInDb));

            var request = new RetrieveApplications();

            var handler = new RetrieveApplicationsCommandHandler(_repository.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.Errors.Should().BeNullOrEmpty();
            result.List.Last().ApplicationStatus.Should().Be("StageTwoRejected");
        }

        private IReadOnlyList<GGSS.Application.Entities.Application> GetApplicationsInDb()
        {
            var applications = new List<GGSS.Application.Entities.Application>
            {
                new GGSS.Application.Entities.Application()
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
                    Organisation = new GGSS.Application.Entities.Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = "Org Name"
                        }
                    },
                    Id = Guid.NewGuid()
                },
                new GGSS.Application.Entities.Application()
                {
                    Value = new ApplicationValue()
                    {
                        StageOne = new StageOneValue()
                        {
                            TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                            {
                                PlantName = "ACME Plant"
                            }
                        }
                    },
                    Organisation = new GGSS.Application.Entities.Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = "ACME"
                        }
                    },
                    Id = Guid.NewGuid()
                },
                new GGSS.Application.Entities.Application()
                {
                    Value = new ApplicationValue()
                    {
                        Status = ApplicationStatus.Rejected,
                        StageOne = new StageOneValue()
                        {
                            TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                            {
                                PlantName = "Super Cool Plant"
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
                    Organisation = new GGSS.Application.Entities.Organisation()
                    {
                        Value = new OrganisationValue()
                        {
                            Name = "Super Cool Org"
                        }
                    },
                    Id = Guid.NewGuid()
                }
            };

            return applications.AsReadOnly();
        }
    }
}
