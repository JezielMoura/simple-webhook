using SimpleWebhook.Domain.RequestAggregate;
using SimpleWebhook.Infrastructure.Persistence.Contexts;

namespace SimpleWebhook.Infrastructure.Persistence.Repositories;

public sealed class EfRequestRepository(AppDbContext context) : IRequestRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Request>> Find(Guid webhookId)
    {
        return await _context.Set<Request>()
            .AsNoTracking()
            .Where(x => x.WebhookId == webhookId)
            .OrderByDescending(x => x.ReceivedAt)
            .ToListAsync();
    }

    public async Task<Guid> Add(Request request)
    {
        _context.Set<Request>().Add(request);
        await _context.SaveChangesAsync();
        return request.Id;
    }
}
