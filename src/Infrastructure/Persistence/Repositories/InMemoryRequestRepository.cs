using System.Collections.Concurrent;
using SimpleWebhook.Domain.RequestAggregate;

namespace SimpleWebhook.Infrastructure.Persistence.Repositories;

public class InMemoryRequestRepository : IRequestRepository
{
    private readonly ConcurrentBag<Request> _requests = [];

    public Task<IEnumerable<Request>> Find(Guid webhookId)
    {
        var filteredRequests = _requests.Where(r => r.WebhookId == webhookId).OrderByDescending(x => x.ReceivedAt);
        return Task.FromResult<IEnumerable<Request>>(filteredRequests);
    }

    public Task<Guid> Add(Request request)
    {
        ArgumentNullException.ThrowIfNull(request);
        _requests.Add(request);
        return Task.FromResult(request.Id);
    }
}