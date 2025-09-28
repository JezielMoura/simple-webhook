using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Webhooks;

public sealed record DeleteWebhookRequest(Guid Id, string? Secret);

public sealed class DeleteWebhookCommand(IWebhookRepository repository)
{
    private readonly IWebhookRepository _repository = repository;

    public async Task<Result<bool, Error>> Execute(DeleteWebhookRequest request)
    {
        var webhook = await _repository.Find(request.Id);

        if (webhook == null || (!string.IsNullOrWhiteSpace(webhook.Secret) && webhook.Secret != request.Secret))
        {
            return new Error { Message = "Cannot execute this operation"};
        }

        await _repository.Delete(request.Id);
        return true;
    }
}
