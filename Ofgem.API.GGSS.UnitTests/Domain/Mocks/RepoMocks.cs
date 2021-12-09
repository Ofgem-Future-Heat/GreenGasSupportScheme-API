using Moq;
using Ofgem.API.GGSS.Application.Contracts.Persistence;
using Ofgem.API.GGSS.Application.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ofgem.API.GGSS.UnitTests.Domain.Mocks
{
    internal class RepoMocks
    {
        public static Mock<IOrganisationRepository> GetOrganisationRepo()
        {
            var userId = Guid.Parse("326fa974-7c05-4b37-a8e4-6d5fe6deb63b");
            var orgId = Guid.Parse("BE9D28DD-199B-4042-B602-4F325586F532");
            var providerId = "F3ACF86B-9F44-44CD-82DF-E10D0E7E7CF8";

            var org = new Organisation
            {
                Id = orgId,
                Value = new GGSS.Domain.ModelValues.OrganisationValue
                {
                    Name = "Ofgem",
                    RegistrationNumber = "1234567",
                    Type = GGSS.Domain.Enums.OrganisationType.Other,
                    RegisteredOfficeAddress = new GGSS.Domain.Models.AddressModel
                    {
                        LineOne = "10 South Colonade",
                        LineTwo = "Canary Wharf",
                        Town = "London",
                        Postcode = "E14 4PU"
                    }
                }
            };

            org.ResponsiblePeople.Add(new ResponsiblePerson
            {
                User = new User
                {
                    Id = userId,
                    ProviderId = providerId,
                    Value = new GGSS.Domain.ModelValues.UserValue
                    {
                        Name = "James",
                        Surname = "Anderson",
                        EmailAddress = "james.anderson@ofgem.gov.uk"
                    }
                },
                Value = new GGSS.Domain.ModelValues.ResponsiblePersonValue
                {
                    TelephoneNumber = "01234567890",
                    DateOfBirth = new DateTime(1965, 8, 17).ToString()
                }
            });

            var orgs = new List<Organisation>{ org };

            var mockCategoryRepository = new Mock<IOrganisationRepository>();
            mockCategoryRepository.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(orgs);

            mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Organisation>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (Organisation org, Guid userId, CancellationToken token) =>
                {
                    orgs.Add(org);
                    return org;
                });

            return mockCategoryRepository;
        }
    }
}
