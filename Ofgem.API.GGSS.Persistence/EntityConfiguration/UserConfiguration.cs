using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;

namespace Ofgem.API.GGSS.Persistence.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public static Guid SeedUserId { get; internal set; }

        public static string SeedUserProviderId { get; internal set; }

        public UserConfiguration()
        {
            SeedUserId = Guid.Parse("F3ACF86B-9F44-44CD-82DF-E10D0E7E7CF8");
            SeedUserProviderId = "326FA974-7C05-4B37-A8E4-6D5FE6DEB63B";
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            builder.HasKey("Id");
            builder.HasIndex("ProviderId").IsUnique();
            builder.HasData(new User
            {
                Id = SeedUserId,
                ProviderId = SeedUserProviderId,
                Value = new UserValue
                {
                    Name = "James",
                    Surname = "Anderson",
                    EmailAddress = "james.anderson@ofgem.gov.uk"
                }
            });

            builder.ToTable("Users");
        }
    }
}
