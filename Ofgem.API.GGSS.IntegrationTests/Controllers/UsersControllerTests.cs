﻿using FluentAssertions;
using Ofgem.API.GGSS.Domain.Commands.Users;
using Ofgem.API.GGSS.IntegrationTests.Base;
using Ofgem.API.GGSS.WebApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Users;
using Xunit;

namespace Ofgem.API.GGSS.IntegrationTests.Controllers
{
    public class UsersControllerTests : IClassFixture<WebApiFactory<Startup>>
    {
        private readonly WebApiFactory<Startup> _factory;

        public UsersControllerTests(WebApiFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddUser_ReturnsIdOfCreatedUser()
        {
            var client = _factory.GetAuthorisedClient();

            var data = new AddUser()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Name = "Test",
                Surname = "User",
                Email = "test.user@ofgem.gov.uk"
            };

            var result = await client.PostAsJsonAsync("/users", data);
            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<AddUserResponse>(content);

            response.UserId.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AddUser_ReturnsIdWithoutErrorIfUserExists()
        {
            var providerId = Guid.NewGuid().ToString();// "";

            var data = new AddUser()
            {
                ProviderId = providerId,
                Name = "Duplicate Test",
                Surname = "User",
                Email = "test.user@ofgem.gov.uk"
            };

            var client = _factory.GetAuthorisedClient();

            _ = await client.PostAsJsonAsync("/users", data);

            var result = await client.PostAsJsonAsync("/users", data);
            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<AddUserResponse>(content);

            response.UserId.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ReturnsListOfOrganisationsForUser()
        {
            var client = _factory.GetAuthorisedClient();
            var userId = Guid.NewGuid();
            var userRepository = _factory.Services.CreateScope().ServiceProvider.GetService<IUserRepository>();
            var user = await userRepository.AddAsync(new User()
            {
                Id = userId
            });
            var organisationRepository =
                _factory.Services.CreateScope().ServiceProvider.GetService<IOrganisationRepository>();
            var organisation = await organisationRepository.AddAsync(new Organisation()
            {
                Value = new OrganisationValue()
                {
                    Name = "Organisation name",
                }
            });

            var responsiblePersonRepository =
                _factory.Services.CreateScope().ServiceProvider.GetService<IResponsiblePersonRepository>();

            await responsiblePersonRepository.AddAsync(new ResponsiblePerson()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrganisationId = organisation.Id
            });

            var response = await client.GetAsync($"/users/{userId}/organisations");
            var deserialisedResponse = await response.Content.ReadAsAsync<GetOrganisationsForUserResponse>();

            deserialisedResponse.Organisations.Should().Contain(o => o.OrganisationName == organisation.Value.Name);
        }

        [Fact]
        public async Task FindUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var client = _factory.GetAuthorisedClient();
            var result = await client.GetAsync("/users/find?ProviderId=notarealuser");

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
        
        [Fact]
        public async Task FindUser_ReturnsIdOfUser_WhenUserExists()
        {
            var providerId = Guid.NewGuid();
            
            var userRepository = _factory.Services.CreateScope().ServiceProvider.GetService<IUserRepository>();
            var user = await userRepository.AddAsync(new User()
            {
                ProviderId = providerId.ToString()
            });
            
            var client = _factory.GetAuthorisedClient();
            var result = await client.GetAsync($"/users/find?ProviderId={providerId.ToString()}");

            var content = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<FindUserResponse>(content);

            response.UserId.Should().Be(user.Id.ToString());
        }
    }
}
