using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Ofgem.API.GGSS.IntegrationTests.Base;
using Ofgem.API.GGSS.Persistence.EntityConfiguration;
using Ofgem.API.GGSS.WebApi;
using Xunit;

namespace Ofgem.API.GGSS.IntegrationTests.Controllers
{
    public class ApplicationControllerTests : IClassFixture<WebApiFactory<Startup>>
    {
        private readonly WebApiFactory<Startup> _factory;
        private HttpClient _client;
        private Guid _applicationGuid;
        private RetrieveApplicationResponse _retrieveApplicationResponse;
        private HttpResponseMessage _httpResponse;
        private ApplicationValue _expectedApplicationValue;

        public ApplicationControllerTests(WebApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreatingNewApplicationReturnsId()
        {
            SetUpTheClient();
            await CreateNewApplication();
            EnsureThatAnIdWasReturned();
        }

        [Fact]
        public async Task ReturnsRequestedApplication()
        {
            SetUpTheClient();
            await SeedApplicationInDb();
            await RetrieveApplication();
            EnsureExpectedApplicationWasRetrieved();
        }

        [Fact]
        public async Task ReturnsNotFoundIfRequestedApplicationDoesntExist()
        {
            SetUpTheClient();
            await AttemptToRetrieveAnApplicationThatDoesntExist();
            EnsureNotFoundStatusCodeWasReturned();
        }
        
        [Fact]
        public async Task UpdatedApplicationsArePersistedToTheDatabase()
        {
            SetUpTheClient();
            await CreateNewApplication();
            await UpdateApplicationWithNewData();
            await RetrieveApplication();
            EnsureExpectedApplicationWasRetrieved();
        }
        
        [Fact]
        public async Task ReturnsNotFoundIfApplicationBeingUpdatedDoesntExist()
        {
            SetUpTheClient();
            await AttemptToUpdateApplicationThatDoesntExist();
            EnsureNotFoundStatusCodeWasReturned();
        }

        private void EnsureThatAnIdWasReturned()
        {
            _applicationGuid.Should().NotBeEmpty();
        }

        private void EnsureExpectedApplicationWasRetrieved()
        {
            _retrieveApplicationResponse.Application.StageOne.TellUsAboutYourSite.PlantName.Should()
                .Be(_expectedApplicationValue.StageOne.TellUsAboutYourSite.PlantName);
        }

        private void EnsureNotFoundStatusCodeWasReturned()
        {
            _httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task CreateNewApplication()
        {
            var data = new
            {
                userId = Guid.NewGuid(),
                organisationId = OrganisationConfiguration.SeedOrganisationId
            };

            var response = await _client.PostAsJsonAsync("/application", data);
            var deserialisedResponse = await response.Content.ReadAsAsync<Dictionary<string, string>>();

            _applicationGuid = new Guid(deserialisedResponse["newApplicationId"]);
        }

        private async Task RetrieveApplication()
        {
            var response = await _client.GetAsync($"/application/{_applicationGuid.ToString()}");
            _retrieveApplicationResponse = await response.Content.ReadAsAsync<RetrieveApplicationResponse>();
        }

        private async Task SeedApplicationInDb()
        {
            _expectedApplicationValue = new ApplicationValue()
            {
                StageOne = new StageOneValue
                {
                    TellUsAboutYourSite = new TellUsAboutYourSiteValue
                    {
                        PlantName = $"My special test plant {Guid.NewGuid()}"
                    }
                }
            };
            
            var appRepository = _factory.Services.CreateScope().ServiceProvider.GetService<IApplicationRepository>();
            var createdApplication = await appRepository.AddAsync(new Application.Entities.Application(_expectedApplicationValue)
            {
                OrganisationId = OrganisationConfiguration.SeedOrganisationId
            });

            _applicationGuid = createdApplication.Id;
        }

        private async Task AttemptToRetrieveAnApplicationThatDoesntExist()
        {
            _applicationGuid = Guid.NewGuid();
            _httpResponse = await _client.GetAsync($"/application/{_applicationGuid.ToString()}");
        }

        private async Task UpdateApplicationWithNewData()
        {
            _expectedApplicationValue = new ApplicationValue()
            {
                StageOne = new StageOneValue
                {
                    TellUsAboutYourSite = new TellUsAboutYourSiteValue
                    {
                        PlantName = $"My special test plant {Guid.NewGuid()}"
                    }
                }
            };
            
            var data = new
            {
                application = _expectedApplicationValue,
                
                userId = Guid.NewGuid(),
            };

            _httpResponse = await _client.PutAsJsonAsync($"/application/{_applicationGuid.ToString()}", data);
        }

        private async Task AttemptToUpdateApplicationThatDoesntExist()
        {
            _applicationGuid = Guid.Empty;
            _httpResponse = await _client.PutAsJsonAsync($"/application/{_applicationGuid.ToString()}", new CreateNewApplicationRequestModel());
        }

        private void SetUpTheClient()
        {
            _client = _factory.GetAuthorisedClient();
        }
    }
}