using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.GGSS.Application.Entities;
using Ofgem.API.GGSS.Domain.ModelValues;
using System;

namespace Ofgem.API.GGSS.Persistence.EntityConfiguration
{
    public class ResponsiblePersonConfiguration : IEntityTypeConfiguration<ResponsiblePerson>
    {
        public static Guid SeedResponsiblePersonId { get; internal set; }

        public ResponsiblePersonConfiguration()
        {
            SeedResponsiblePersonId = Guid.Parse("23D389D1-5A79-40E4-8171-D829B3244C6D");
        }

        public void Configure(EntityTypeBuilder<ResponsiblePerson> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            builder.HasKey(p => new { p.OrganisationId, p.UserId }).HasName("Organisation_ResponsiblePerson");
            builder.HasIndex(p => new { p.UserId, p.OrganisationId }).IsUnique();
            builder.HasOne(p => p.User).WithMany(u => u.ResponsiblePeople).HasForeignKey(rp => rp.UserId).HasConstraintName("FK_Users_ResponsiblePerson");
            builder.HasOne(p => p.Organisation).WithMany(o => o.ResponsiblePeople).HasForeignKey(rp => rp.OrganisationId).HasConstraintName("FK_Organisations_ResponsiblePerson");
            builder.HasData(new ResponsiblePerson
            {
                Id = SeedResponsiblePersonId,
                UserId = UserConfiguration.SeedUserId,
                OrganisationId = OrganisationConfiguration.SeedOrganisationId,
                Value = new ResponsiblePersonValue
                {
                    TelephoneNumber = "01234567890",
                    DateOfBirth = new DateTime(1965, 8, 17).ToString()
                }
            });
            builder.ToTable("ResponsiblePeople");
        }
    }
}
