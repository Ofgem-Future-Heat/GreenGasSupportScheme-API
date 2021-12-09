using System;
using FluentAssertions;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.IntegrationTests.Base;
using Ofgem.API.GGSS.Persistence.EntityConfiguration;
using Ofgem.API.GGSS.WebApi;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Organisations;
using Xunit;

namespace Ofgem.API.GGSS.IntegrationTests.Controllers
{
    public class OrganisationsControllerTests : IClassFixture<WebApiFactory<Startup>>
    {
        private readonly WebApiFactory<Startup> _factory;
        private HttpResponseMessage _httpResponse;
        private HttpClient _client;

        public OrganisationsControllerTests(WebApiFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.GetAuthorisedClient();
        }

        [Fact(Skip = "To be removed when Redis is removed")]
        public async Task AddWithResponsiblePerson_ReturnsSuccessResult()
        {
            // Arrange 
            var requestData = new OrganisationSave();

            // Act
            _httpResponse = await _client.PostAsync("/Organisations/AddWithResponsiblePerson", requestData, default);

            // Assert
            _httpResponse.EnsureSuccessStatusCode();

            var responseString = await _httpResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<string>(responseString);

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetListOfOrganisations()
        {
            var organisationRepository =
                _factory.Services.CreateScope().ServiceProvider.GetService<IOrganisationRepository>();
            var createdOrganisation = await organisationRepository.AddAsync(new Organisation()
            {
                Id = new Guid(),
                Value = new OrganisationValue()
                {
                    Name = "Organisation Name"
                }
            });

            var serviceResponse = await _client.GetAsync("Organisations/getAllOrganisations");
            var deserialisedResponse = await serviceResponse.Content.ReadAsAsync<RetrieveOrganisationsResponse>();

            deserialisedResponse.Organisations.Should().Contain(o => o.OrganisationName == createdOrganisation.Value.Name);
        }
    }
}
