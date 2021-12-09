using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.IntegrationTests.Base;
using Ofgem.API.GGSS.Persistence.EntityConfiguration;
using Ofgem.API.GGSS.WebApi;
using Xunit;

namespace Ofgem.API.GGSS.IntegrationTests.Controllers
{
    public class ApplicationsControllerTests : IClassFixture<WebApiFactory<Startup>>
    {
        private readonly WebApiFactory<Startup> _factory;

        public ApplicationsControllerTests(WebApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SaveStageOne_ReturnsApplicationId()
        {
            var client = _factory.GetAuthorisedClient();

            var data = new
            {
                InstallationName = "Test",
                OrganisationId = OrganisationConfiguration.SeedOrganisationId,
                DateInjectionStart = DateTime.UtcNow.AddMonths(7),
                Location = Location.England.ToString(),
                MaxCapacity = 250,
                InstallationAddress = new AddressModel
                {
                    LineOne = "Test",
                    Town = "Town",
                    Postcode = "AB123CD"
                },
                CapacityCheckDocument = new Domain.ModelValues.DocumentValue
                {
                    FileName = "1",
                    FileId = "1"
                },
                PlanningPermissionDocument = new Domain.ModelValues.DocumentValue
                {
                    FileName = "3",
                    FileId = "3"
                }
            };

            var response = await client.PostAsJsonAsync("/applications/stageone", data);

            response.EnsureSuccessStatusCode();

            var id = await response.Content.ReadAsStringAsync();

            id
                .Should()
                .NotBeNull();
            id
                .Should()
                .BeOfType(typeof(string));
        }
    }
}
