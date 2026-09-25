using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Languages.Infrastructure.Persistence.Configurations;

internal abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.ID);

        builder.Property(e => e.ID)
            .ValueGeneratedOnAdd();
        
        builder.Property(e => e.CreatedAtUTC)
            .IsRequired();
        
        builder.Property(e => e.ModifiedAtUTC)
            .IsRequired();
    }
}