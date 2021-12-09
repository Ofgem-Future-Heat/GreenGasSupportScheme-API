using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.GGSS.Application.Entities;
using System;

namespace Ofgem.API.GGSS.Persistence.EntityConfiguration
{
    public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
    {
        public static Guid SeedOrganisationId { get; internal set; }

        public OrganisationConfiguration()
        {
            SeedOrganisationId = Guid.Parse("B141AC41-D6F7-47FD-B31E-847D77134FCA");
        }

        public void Configure(EntityTypeBuilder<Organisation> builder)
        {
            builder.Property(o => o.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            builder.HasKey(o => o.Id);
            builder.HasMany(o => o.Applications).WithOne(a => a.Organisation).HasForeignKey(a => a.OrganisationId);
            builder.HasData(new Organisation
            {
                Id = SeedOrganisationId,
                Value = new Domain.ModelValues.OrganisationValue
                {
                    Name = "Clydebiomass UK",
                    RegistrationNumber = "1234567",
                    Type = Domain.Enums.OrganisationType.Private,
                    ReferenceNumber = Guid.NewGuid().ToString(),
                    RegisteredOfficeAddress = new Domain.Models.AddressModel
                    {
                        LineOne = "10 South Colonade",
                        LineTwo = "Canary Wharf",
                        Town = "London",
                        Postcode = "E14 4PU"
                    }
                }
            });

            builder.ToTable("Organisations");
        }
    }
}
