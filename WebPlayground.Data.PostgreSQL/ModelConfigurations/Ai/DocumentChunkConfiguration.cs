using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgvector;
using WebPlayground.Data.Models.Ai;

namespace WebPlayground.Data.PostgreSQL.ModelConfigurations.Ai
{
    public class DocumentChunkConfiguration : IEntityTypeConfiguration<DocumentChunk>
    {
        public void Configure(EntityTypeBuilder<DocumentChunk> builder)
        {
            builder.ToTable("DocumentChunks", "ai");
            
            builder.Property(e => e.Embedding)
                .HasColumnType("vector(1536)")
                .HasConversion(
                    floatArray => new Vector(floatArray),
                    vector => vector.ToArray() 
                );
        }
    }
}
