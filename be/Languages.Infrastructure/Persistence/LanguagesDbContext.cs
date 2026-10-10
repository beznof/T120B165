using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Infrastructure.Persistence;

public sealed class LanguagesDbContext(DbContextOptions<LanguagesDbContext> options) : DbContext(options), ILanguagesDbContext
{
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Dictionary> Dictionaries => Set<Dictionary>();
    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LanguagesDbContext).Assembly);
    }
}
