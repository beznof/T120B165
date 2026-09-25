using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal sealed class LanguageConfiguration : BaseEntityConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Languages");
        
        builder.Property(l => l.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.BackgroundImageUrl)
            .HasMaxLength(2000);
        
        builder.HasOne(l => l.LanguageFamily)
            .WithMany(f => f.Languages)
            .HasForeignKey(l => l.LanguageFamilyID);
        
        builder.HasMany(l => l.LanguageLevels)
            .WithOne()
            .HasForeignKey(l => l.LanguageID);
    }
}
