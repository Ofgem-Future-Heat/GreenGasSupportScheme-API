using AutoMapper;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Application.Profiles;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.UnitTests.Domain.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Domain.Commands
{
    public class OrganisationSaveTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IOrganisationRepository> _mockOrganisationRepository;

        public OrganisationSaveTests()
        {
            _mockOrganisationRepository = RepoMocks.GetOrganisationRepo();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidOrganisation_AddedToOrganisationsRepo()
        {
            var handler = new OrganisationSaveCommandHandler(_mapper, _mockOrganisationRepository.Object);

            var userId = "326fa974-7c05-4b37-a8e4-6d5fe6deb63b";
           
            var newOrg = new OrganisationSave()
            {
                UserId = userId,
                Model = new OrganisationModel
                {
                    ResponsiblePeople = new List<ResponsiblePersonModel>
                    {
                         new ResponsiblePersonModel
                        {
                            Value = new GGSS.Domain.ModelValues.ResponsiblePersonValue
                            {
                                TelephoneNumber = "01234567890",
                                DateOfBirth = new DateTime(1965, 8, 17).ToString()
                            },
                            User = new UserModel
                            {
                                ProviderId = "a9b3a625-1d0d-4d13-8f20-17098db638fe",
                                Value = new GGSS.Domain.ModelValues.UserValue
                                {
                                    Name = "James",
                                    Surname = "Anderson",
                                    EmailAddress = "james.anderson@ofgem.gov.uk"
                                },
                                IsResponsiblePerson = false
                            }
                        }
                    },
                    Value = new GGSS.Domain.ModelValues.OrganisationValue
                    {
                        Name = "Dummy",
                        RegistrationNumber = "9999999",
                        Type = GGSS.Domain.Enums.OrganisationType.Private,
                        RegisteredOfficeAddress = new GGSS.Domain.Models.AddressModel
                        {
                            LineOne = "Dummy address",
                            Town = "Some city",
                            Postcode = "AB12 3CD"
                        }
                    }
                }
            };

            await handler.Handle(newOrg, CancellationToken.None);

            var allOrgs = await _mockOrganisationRepository.Object.ListAllAsync();
            allOrgs.Count.Should().Be(2);
        }
    }
}
