using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Persistence;
using System;
using System.Collections.Generic;

namespace Ofgem.API.GGSS.IntegrationTests.Base
{
    public class Utilities
    {
        public static Guid userId = Guid.Parse("E426AC5C-A228-45B4-BF88-436EE969C1E2");
        public static string userProviderId = "FA7DD270-653D-4D06-8CA0-72939CFDB4D9";
        public static Guid orgId = Guid.Parse("3A99989F-7A67-433F-A981-A944642A1BB7");

        public static void InitializeDbForTests(ApplicationDbContext context)
        {
            var user = new User
            {
                Id = userId,
                ProviderId = userProviderId,
                Value = new Domain.ModelValues.UserValue
                {
                    Name = "Test",
                    Surname = "User",
                    EmailAddress = "test.user@ofgem.gov.uk"
                }
            };

            context.Users.Add(user);

            context.Organisations.Add(new Application.Entities.Organisation
            {
                Id = orgId,
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
                },
                ResponsiblePeople = new List<ResponsiblePerson>
                {
                    new ResponsiblePerson
                    {
                        User = user,
                        Value = new Domain.ModelValues.ResponsiblePersonValue
                        {
                            TelephoneNumber = "01234567890",
                            DateOfBirth = new DateTime(1965, 8, 17).ToString()
                        }
                    }
                }
            });

            context.SaveChanges();
        }
    }
}
