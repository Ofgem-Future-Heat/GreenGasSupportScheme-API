using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.GGSS.Application.Entities;

namespace Ofgem.API.GGSS.Persistence.EntityConfiguration
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.Property(d => d.Id).ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
            builder.HasKey(d => d.Id);
            builder.HasIndex(d => d.ApplicationId);
            builder.HasOne(d => d.Application).WithMany(a => a.Documents).HasForeignKey(d => d.ApplicationId);
            builder.ToTable("Documents");
        }
    }
}
