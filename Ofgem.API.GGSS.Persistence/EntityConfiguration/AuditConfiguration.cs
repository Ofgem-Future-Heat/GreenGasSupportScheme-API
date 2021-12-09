using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.GGSS.Application.Entities;

namespace Ofgem.API.GGSS.Persistence.EntityConfiguration
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.Property(a => a.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            builder.HasKey(a => a.Id);
            builder.ToTable("AuditLogs");
        }
    }
}
