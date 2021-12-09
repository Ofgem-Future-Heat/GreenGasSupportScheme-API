using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ofgem.API.GGSS.Persistence.EntityConfiguration
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application.Entities.Application>
    {
        public void Configure(EntityTypeBuilder<Application.Entities.Application> builder)
        {
            builder.Property(a => a.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.OrganisationId);
            builder.HasOne(a => a.Organisation).WithMany(o => o.Applications).HasForeignKey(a => a.OrganisationId);
            builder.ToTable("Applications");
        }
    }
}
