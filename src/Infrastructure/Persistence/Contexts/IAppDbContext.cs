namespace SimpleWebhook.Infrastructure.Persistence.Contexts;

public interface IAppDbContext
{
    Task<bool> Commit(CancellationToken cancellationToken = default);
}
