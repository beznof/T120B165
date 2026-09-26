using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal sealed class DictionaryEntryConfiguration : BaseEntityConfiguration<DictionaryEntry>
{
    public override void Configure(EntityTypeBuilder<DictionaryEntry> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("DictionaryEntries");

        builder.Property(e => e.Text)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.Translation)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.Type)
            .HasConversion<string>();

        builder.Property(e => e.PhoneticTranscription)
            .HasMaxLength(500);

        builder.Property(e => e.DictionaryId)
            .IsRequired();

        builder.HasOne(e => e.Dictionary)
            .WithMany(d => d.Entries)
            .HasForeignKey(e => e.DictionaryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.Synonyms)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
