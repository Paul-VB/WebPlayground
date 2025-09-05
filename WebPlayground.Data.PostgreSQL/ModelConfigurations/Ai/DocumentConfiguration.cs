using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebPlayground.Data.Models.Ai;

namespace WebPlayground.Data.PostgreSQL.ModelConfigurations.Ai
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents", "ai");
        }
    }
}