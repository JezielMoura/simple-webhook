namespace SimpleWebhook.Domain.RequestAggregate;

public interface IRequestRepository
{
    Task<IEnumerable<Request>> Find(Guid webhookId);
    Task<Guid> Add(Request request);
}
