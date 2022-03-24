using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using Ofgem.API.GGSS.IntegrationTests.Base;
using Ofgem.API.GGSS.WebApi;
using Xunit;

namespace Ofgem.API.GGSS.IntegrationTests.Controllers
{
    public class OrganisationControllerTests : IClassFixture<WebApiFactory<Startup>>
    {
        private readonly WebApiFactory<Startup> _factory;
        private HttpClient _client;
        private Guid _organisationId;
        private Guid _applicationId;

        public OrganisationControllerTests(WebApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsNotFoundIfOrganisationDoesntExist()
        {
            SetUpTheClient();

            var response = await _client.GetAsync($"/organisation/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
        
        [Fact]
        public async Task ReturnsOrganisationWithListOfApplicationsWhenFound()
        {
            await SeedOrganisationInDb();
            SetUpTheClient();
            
            var response = await _client.GetAsync($"/organisation/{_organisationId.ToString()}");

            var deserialisedResponse = await response.Content.ReadAsAsync<RetrieveOrganisationResponse>();

            deserialisedResponse.Applications.First().Id.Should().Be(_applicationId.ToString());

        }
        
        
        [Fact]
        public async Task GetOrganisationDetails()
        {
            var organisationRepository =
                _factory.Services.CreateScope().ServiceProvider.GetService<IOrganisationRepository>();
            
            var createdOrganisation = await organisationRepository.AddAsync(new Organisation()
            {
                Id = new Guid(),
                Value = new OrganisationValue()
                {
                    Name = "Organisation Name",
                    Type = OrganisationType.Private,
                    RegistrationNumber = "1234567",
                    RegisteredOfficeAddress = new AddressModel()
                }
            });
            
            _organisationId = createdOrganisation.Id;
            SetUpTheClient();

            var response = await _client.GetAsync($"Organisation/{_organisationId.ToString()}/details");
            
            var deserialisedResponse = await response.Content.ReadAsStringAsync();
            
            response.EnsureSuccessStatusCode();
            deserialisedResponse.Should().Contain(createdOrganisation.Value.Name);
            deserialisedResponse.Should().Contain(createdOrganisation.Value.RegistrationNumber);
            deserialisedResponse.Should().Contain(createdOrganisation.Value.Type.ToString());
        }

        [Fact]
        public async Task UpdateOrganisationStatus()
        {
            await SeedOrganisationInDb();
            SetUpTheClient();

            var data = new
            {
                OrganisationStatus = "Verified",
            };

            var request = new HttpRequestMessage()
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri($"{_client.BaseAddress}Organisation/{_organisationId}/details"),
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.Default, "application/json"),
            };

            await _client.SendAsync(request);
            
            var response = await _client.GetAsync($"Organisation/{_organisationId}/details");

            var updatedOrganisation = JsonConvert.DeserializeObject<RetrieveOrganisationDetailsResponse>(await response.Content.ReadAsStringAsync());

            updatedOrganisation.OrganisationStatus.Should().Be("Verified");
            DateTime.Parse(updatedOrganisation.LastModified).Should().BeAfter(new DateTime(2020, 01, 01));
        }
        
        private void SetUpTheClient()
        {
            _client = _factory.GetAuthorisedClient();
        }
        private async Task SeedOrganisationInDb()
        {
            var repository = _factory.Services.CreateScope().ServiceProvider.GetService<IOrganisationRepository>();
            var createdOrganisation = await repository.AddAsync(new Organisation()
            {
                Value = new OrganisationValue()
                {
                    LastModified = new DateTime(2020, 01, 01).ToString("s")
                },
                Applications = new List<Application.Entities.Application>()
                {
                    new Application.Entities.Application()
                    {
                        Value = new ApplicationValue()
                    }
                }
            });

            _organisationId = createdOrganisation.Id;
            _applicationId = createdOrganisation.Applications.First().Id;
        }
        
    }
}