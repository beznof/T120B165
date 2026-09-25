using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal sealed class DictionaryEntrySynonymConfiguration : BaseEntityConfiguration<DictionaryEntrySynonym>
{
    public override void Configure(EntityTypeBuilder<DictionaryEntrySynonym> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("DictionaryEntrySynonyms");
        
        builder.Property(s => s.Text)
            .HasMaxLength(500)
            .IsRequired();
    }
}