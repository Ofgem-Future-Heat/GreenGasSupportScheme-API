using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Application.Handlers;
using Ofgem.API.GGSS.Domain.Commands.Organisations;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Xunit;

namespace Ofgem.API.GGSS.UnitTests.Application.Handlers
{
    public class RetrieveOrganisationDetailsCommandHandlerTests
    {
        private readonly Guid _organisationId;
        private Guid _authorisedSignatoryUserId;
        private Guid _adminUserId;

        public RetrieveOrganisationDetailsCommandHandlerTests()
        {
            _organisationId = Guid.NewGuid();
            _authorisedSignatoryUserId = Guid.NewGuid();
            _adminUserId = Guid.NewGuid();
        }
        
        [Fact]
        public async Task ReturnOrganisationDetails()
        {
            var OrganisationId = "1234";
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetOrganisationDetailsByOrgId(OrganisationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Organisation()
                {
                    Value = new OrganisationValue()
                    {
                        Name = "Organisation Name",
                        Type = OrganisationType.Other,
                        RegistrationNumber = "5678"
                    }
                });

            var handler = new RetrieveOrganisationDetailsCommandHandler(repository.Object);
            var request = new RetrieveOrganisationDetails() {OrganisationId = OrganisationId};

            var result = await handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();
            result.OrganisationName.Should().Be("Organisation Name");
            result.OrganisationRegistrationNumber.Should().Be("5678");
        }

        [Fact]
        public async Task ReturnOrganisationUsers()
        {
            var OrganisationId = "1234";
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetOrganisationDetailsByOrgId(OrganisationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Organisation()
                {
                    Value = new OrganisationValue()
                    {
                        Name = "Organisation Name",
                        Type = OrganisationType.Other,
                        RegistrationNumber = "5678"
                    },
                    UserOrganisations = new List<UserOrganisation>
                    {
                        new UserOrganisation()
                        {
                            UserId = Guid.NewGuid(),
                            User = new User()
                            {
                                Value = new UserValue()
                                {
                                    Name = "Unique Name",
                                    Surname = "Unique surname",
                                    EmailAddress = "uniqueperson@email.com"
                                }
                            }
                        }
                    }
                });

            var handler = new RetrieveOrganisationDetailsCommandHandler(repository.Object);
            var request = new RetrieveOrganisationDetails() {OrganisationId = OrganisationId};

            var result = await handler.Handle(request, CancellationToken.None);
            
            result.OrganisationName.Should().Be("Organisation Name");
            result.OrganisationUsers.Should().Contain(u => u.Name == "Unique Name" &&  u.Surname == "Unique surname" && u.EmailAddress == "uniqueperson@email.com");
        }
        
        [Fact]
        public async Task FilterOutPendingOrganisationUsersWhereUserIdIsCurrentlyNull()
        {
            var OrganisationId = "1234";
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetOrganisationDetailsByOrgId(OrganisationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Organisation()
                {
                    Value = new OrganisationValue()
                    {
                        Name = "Organisation Name",
                        Type = OrganisationType.Other,
                        RegistrationNumber = "5678"
                    },
                    UserOrganisations = new List<UserOrganisation>
                    {
                        new UserOrganisation()
                        {
                        }
                    }
                });
            var handler = new RetrieveOrganisationDetailsCommandHandler(repository.Object);
            var request = new RetrieveOrganisationDetails() {OrganisationId = OrganisationId};
            var result = await handler.Handle(request, CancellationToken.None);
            
            result.OrganisationName.Should().Be("Organisation Name");
            result.OrganisationUsers.Should().BeEmpty();
        }
        
        [Fact]
        public async Task UserIsAuthorisedSignatory()
        {
            var repository = new Mock<IOrganisationRepository>();
            repository.Setup(a => a.GetOrganisationDetailsByOrgId(_organisationId.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Organisation()
                {
                    Value = new OrganisationValue()
                    {
                        Name = "Organisation Name",
                        Type = OrganisationType.Other,
                        RegistrationNumber = "5678"
                    },
                    
                    ResponsiblePeople = new List<ResponsiblePerson>()
                    {
                        new ResponsiblePerson()
                        {
                            UserId = _authorisedSignatoryUserId,
                            User = new User()
                            {
                                Value = new UserValue()
                                {
                                    Name = "Name",
                                    Surname = "Surname",
                                    EmailAddress = "email@test.com"
                                }
                            },
                            Value = new ResponsiblePersonValue()
                            {
                                TelephoneNumber = "01234567890"
                            }
                        }
                    },
                    
                    UserOrganisations = new List<UserOrganisation>
                    {
                        new UserOrganisation()
                        {
                            User = new User()
                            {
                                Value = new UserValue()
                                {
                                    Name = "Unique Name",
                                    Surname = "Unique surname",
                                    EmailAddress = "uniqueperson@email.com"
                                }
                            }
                        }
                    }
                });

            var handler = new RetrieveOrganisationDetailsCommandHandler(repository.Object);
            var request = new RetrieveOrganisationDetails()
            {
                OrganisationId = _organisationId.ToString(),
                UserId = _authorisedSignatoryUserId.ToString()
            };

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsAuthorisedSignatory.Should().BeTrue();
        }
    }
}