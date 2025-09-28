using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Webhooks;

public sealed record DeleteWebhookRequest(Guid Id);

public sealed class DeleteWebhookCommand(IWebhookRepository repository)
{
    private readonly IWebhookRepository _repository = repository;

    public async Task<bool> Execute(DeleteWebhookRequest request)
    {
        var webhook = await _repository.Find(request.Id);

        if (webhook == null)
        {
            return false;
        }

        await _repository.Delete(request.Id);
        return true;
    }
}
