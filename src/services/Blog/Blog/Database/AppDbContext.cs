using Microsoft.EntityFrameworkCore;

namespace Blog.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Blog> Blogs => Set<Models.Blog>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .Build();

            var connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}