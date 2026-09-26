using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Infrastructure.Persistence;

public sealed class LanguagesDbContext(DbContextOptions<LanguagesDbContext> options)
    : DbContext(options), ILanguagesDbContext
{
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<LanguageDictionary> Dictionaries => Set<LanguageDictionary>();
    public DbSet<DictionaryEntry> DictionaryEntries => Set<DictionaryEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LanguagesDbContext).Assembly);
    }
}
