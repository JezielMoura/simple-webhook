using SimpleWebhook.Domain.RequestAggregate;
using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Webhooks;

public sealed record GetWebhookRequest(Guid Id, string? Secret);
public sealed record GetWebhookResponse(Guid Id, string Url, IEnumerable<Request> Requests);

public sealed class GetWebhookQuery(IWebhookRepository repository, IRequestRepository requestRepository)
{
    private readonly IWebhookRepository _repository = repository;
    private readonly IRequestRepository _requestRepository = requestRepository;

    public async Task<Result<GetWebhookResponse, Error>> Execute(GetWebhookRequest request)
    {
        var webhook = await _repository.Find(request.Id);

        if (webhook is null || (webhook.Secret != request.Secret && !string.IsNullOrWhiteSpace(webhook.Secret)))
        {
            return new Error() { Message = "Unauthorized" };
        }

        var requests = await _requestRepository.Find(webhook.Id);
        return new GetWebhookResponse(webhook.Id, webhook.Url, requests);
    }
}
