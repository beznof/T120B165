using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal sealed class LanguageLevelConfiguration : BaseEntityConfiguration<LanguageLevel>
{
    public override void Configure(EntityTypeBuilder<LanguageLevel> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("LanguageLevels");
        
        builder.Property(l => l.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}