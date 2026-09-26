using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal sealed class LanguageDictionaryConfiguration : BaseEntityConfiguration<LanguageDictionary>
{
    public override void Configure(EntityTypeBuilder<LanguageDictionary> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("LanguageDictionaries");

        builder.Property(d => d.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.BackgroundImageUrl)
            .HasMaxLength(2000);

        builder.HasOne(d => d.Language)
            .WithMany(l => l.Dictionaries)
            .HasForeignKey(dictionary => dictionary.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
