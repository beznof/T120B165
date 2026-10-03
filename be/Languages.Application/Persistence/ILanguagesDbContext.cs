using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Application.Persistence;

public interface ILanguagesDbContext
{
    public DbSet<Language> Languages { get; }
    public DbSet<LanguageDictionary> Dictionaries { get; }
    public DbSet<DictionaryEntry> DictionaryEntries { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}