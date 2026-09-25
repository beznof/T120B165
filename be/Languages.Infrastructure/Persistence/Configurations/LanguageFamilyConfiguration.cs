using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal sealed class LanguageFamilyConfiguration : BaseEntityConfiguration<LanguageFamily>
{
    public override void Configure(EntityTypeBuilder<LanguageFamily> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("LanguageFamilies");
        
        builder.Property(l => l.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}