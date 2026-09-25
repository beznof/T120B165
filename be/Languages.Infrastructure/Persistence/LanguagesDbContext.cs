using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Infrastructure.Persistence;

public sealed class LanguagesDbContext(DbContextOptions<LanguagesDbContext> options) : DbContext(options)
{
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<LanguageDictionary> Dictionaries => Set<LanguageDictionary>();
    public DbSet<DictionaryEntry> DictionaryEntries => Set<DictionaryEntry>();
    public DbSet<LanguageFamily> LanguageFamilies => Set<LanguageFamily>();
    public DbSet<LanguageLevel> LanguageLevels => Set<LanguageLevel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LanguagesDbContext).Assembly);
    }
}
