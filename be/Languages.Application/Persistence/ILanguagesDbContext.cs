using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Application.Persistence;

public interface ILanguagesDbContext
{
    public DbSet<Language> Languages { get; }
    public DbSet<Dictionary> Dictionaries { get; }
    public DbSet<Entry> Entries { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}