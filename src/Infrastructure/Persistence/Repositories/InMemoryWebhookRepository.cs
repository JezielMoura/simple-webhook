using System.Collections.Concurrent;
using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Infrastructure.Persistence.Repositories;

public class InMemoryWebhookRepository : IWebhookRepository
{
    private readonly ConcurrentDictionary<Guid, Webhook> _webhooks = new();

    public Task<Webhook?> Find(Guid id)
    {
        _webhooks.TryGetValue(id, out var webhook);
        return Task.FromResult(webhook);
    }

    public Task Add(Webhook webhook)
    {
        ArgumentNullException.ThrowIfNull(webhook);

        if (!_webhooks.TryAdd(webhook.Id, webhook))
        {
            throw new InvalidOperationException($"Webhook with ID {webhook.Id} already exists.");
        }

        return Task.CompletedTask;
    }

    public Task Delete(Guid id)
    {
        _webhooks.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Webhook>> List()
    {
        return Task.FromResult(_webhooks.Select(x => x.Value));
    }
}