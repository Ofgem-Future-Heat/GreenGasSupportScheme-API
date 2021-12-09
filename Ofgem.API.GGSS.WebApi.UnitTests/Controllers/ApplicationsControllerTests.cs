using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ofgem.API.GGSS.Domain.Commands.Applications;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Ofgem.API.GGSS.WebApi.Controllers;
using Xunit;

namespace Ofgem.API.GGSS.WebApi.UnitTests
{
    public class ApplicationsControllerTests
    {
        [Fact]
        public async Task ShouldReturnOkWithResponse()
        {
            var mediator = new Mock<IMediator>();

            mediator
                .Setup(m => m.Send(It.IsAny<RetrieveApplications>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new RetrieveApplicationsResponse()));

            var controller = new ApplicationsController(mediator.Object);

            var action = await controller.GetAllApplications(CancellationToken.None);

            var result = (OkObjectResult)action;

            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWithResponse()
        {
            var response = new RetrieveApplicationsResponse();

            response.Errors.Add("error-collection-item");

            var mediator = new Mock<IMediator>();

            mediator
                .Setup(m => m.Send(It.IsAny<RetrieveApplications>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var controller = new ApplicationsController(mediator.Object);

            var action = await controller.GetAllApplications(CancellationToken.None);

            var result = (NotFoundObjectResult)action;

            result.StatusCode.Should().Be(404);
        }
    }
}
