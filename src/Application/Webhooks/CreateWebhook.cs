using Microsoft.AspNetCore.Http;
using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Webhooks;

public sealed record CreateWebHookRequest(string Secret);

public sealed class CreateWebhookCommand(IHttpContextAccessor accessor, IWebhookRepository repository)
{
    private readonly IHttpContextAccessor _accessor = accessor;
    private readonly IWebhookRepository _repository = repository;

    public async Task<string> Execute(CreateWebHookRequest request)
    {
        var id = Guid.NewGuid();
        var webhook = new Webhook
        {
            Id = id,
            Secret = request.Secret,
            Url = $"https://{_accessor.HttpContext?.Request.Host}/api/{id}",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.Add(webhook);

        return webhook.Url;
    }
}
