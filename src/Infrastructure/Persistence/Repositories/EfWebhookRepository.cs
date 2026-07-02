using SimpleWebhook.Domain.WebhookAggregate;
using SimpleWebhook.Infrastructure.Persistence.Contexts;

namespace SimpleWebhook.Infrastructure.Persistence.Repositories;

public sealed class EfWebhookRepository(AppDbContext context) : IWebhookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Webhook?> Find(Guid id)
    {
        return await _context.Set<Webhook>().FindAsync(id);
    }

    public async Task<IEnumerable<Webhook>> List()
    {
        return await _context.Set<Webhook>()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task Add(Webhook webhook)
    {
        _context.Set<Webhook>().Add(webhook);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        Webhook? webhook = await _context.Set<Webhook>().FindAsync(id);
        if (webhook is not null)
        {
            _context.Set<Webhook>().Remove(webhook);
            await _context.SaveChangesAsync();
        }
    }
}
