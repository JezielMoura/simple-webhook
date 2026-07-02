using SimpleWebhook.Domain.RequestAggregate;
using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Infrastructure.Persistence.Contexts;

public sealed class AppDbContext(IConfiguration configuration) : DbContext, IAppDbContext, IUnitOfWork
{
    private readonly IConfiguration _configuration = configuration;

    public DbSet<Webhook> Webhooks => Set<Webhook>();
    public DbSet<Request> Requests => Set<Request>();


    public async Task<bool> Commit(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = _configuration["POSTGRES"];
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.UseNpgsql(connectionString ?? throw new ArgumentException("Missing connection string"), options =>
        {
            options.ConfigureDataSource(x => x.EnableDynamicJson());
        });
        base.OnConfiguring(optionsBuilder);
    }
}
