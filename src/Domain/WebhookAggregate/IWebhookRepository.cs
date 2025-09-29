namespace SimpleWebhook.Domain.WebhookAggregate;

public interface IWebhookRepository
{
    Task<Webhook?> Find(Guid id);
    Task<IEnumerable<Webhook>> List();
    Task Add(Webhook webhook);
    Task Delete(Guid id);
}
