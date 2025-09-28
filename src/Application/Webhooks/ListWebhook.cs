using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Webhooks;

public sealed record ListWebhookResponse(IEnumerable<GetWebhookResponse> Webhooks);

public sealed class ListWebhookQuery(IWebhookRepository repository)
{
    private readonly IWebhookRepository _repository = repository;

    public async Task<ListWebhookResponse?> Execute()
    {
        var webhooks = await _repository.List();
        var response = webhooks.OrderByDescending(x => x.CreatedAt).Select(x => new GetWebhookResponse(x.Id, x.Url, []));
        return new ListWebhookResponse(response);
    }
}
