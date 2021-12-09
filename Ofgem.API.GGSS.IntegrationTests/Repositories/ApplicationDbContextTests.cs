using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Ofgem.API.GGSS.Application.Contracts;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Persistence;
using System;
using System.Linq;
using Xunit;

namespace Ofgem.API.GGSS.IntegrationTests.Repositories
{
    public class ApplicationDbContextTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private readonly string _loggedInUserId;

        private readonly Guid _orgId;

        public ApplicationDbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _loggedInUserId = "C4AF3613-021B-4980-AB91-7A3363B9C113";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_loggedInUserId);
            _orgId = Guid.NewGuid();

            _context = new ApplicationDbContext(dbContextOptions, _loggedInUserServiceMock.Object);
        }

        [Fact]
        public async void Save_Organisation_WithUserAsResponsiblePerson()
        {
            _context.ResponsiblePeople.Add(new ResponsiblePerson
            {
                Id = Guid.NewGuid(),
                OrganisationId = _orgId,
                Value = new Domain.ModelValues.ResponsiblePersonValue
                {
                    TelephoneNumber = "01234567890",
                    DateOfBirth = new DateTime(1965, 8, 17).ToString()
                },
                User = new User
                {
                    Id = Guid.Parse(_loggedInUserId),
                    Value = new Domain.ModelValues.UserValue
                    {
                        Name = "Test",
                        Surname = "User",
                        EmailAddress = "test.user@ofgem.gov.uk"
                    }
                }
            });

            _context.Organisations.Add(new Organisation()
            {
                Id = _orgId,               
                Value = new Domain.ModelValues.OrganisationValue
                {
                    Name = "Ofgem",
                    RegistrationNumber = "1234567",
                    Type = Domain.Enums.OrganisationType.Other,
                    RegisteredOfficeAddress = new Domain.Models.AddressModel
                    {
                        LineOne = "10 South Colonade",
                        LineTwo = "Canary Wharf",
                        Town = "London",
                        Postcode = "E14 4PU"
                    }
                }
            });

            await _context.SaveChangesAsync();

            var saved = await _context.Organisations.AnyAsync();
            var found = await _context.Organisations.FirstOrDefaultAsync(o => o.Id == _orgId);

            saved.Should().BeTrue();
            found.Should().NotBeNull();
            found.Value.Should().NotBeNull();
            found.Value.Name.Should().BeEquivalentTo("Ofgem");
            found.ResponsiblePeople.Any().Should().BeTrue();
            found.ResponsiblePeople.FirstOrDefault(p => p.OrganisationId == _orgId).Should().NotBeNull();
            found.ResponsiblePeople.Select(p => p.User).Should().NotBeNull();
            found.ResponsiblePeople.Select(p => p.User.Id).Should().BeEquivalentTo(Guid.Parse(_loggedInUserId));
        }
    }
}
