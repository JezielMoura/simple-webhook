using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;
using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Webhooks;

public sealed record CreateWebHookRequest(string Secret);

public sealed class CreateWebhookCommand(IHttpContextAccessor accessor, IWebhookRepository repository, Tracer tracer, Meter meter)
{
    private readonly IHttpContextAccessor _accessor = accessor;
    private readonly IWebhookRepository _repository = repository;
    private readonly Tracer _tracer = tracer;
    private readonly Counter<int> _counter = meter.CreateCounter<int>("webhook.createds");

    public async Task<string> Execute(CreateWebHookRequest request)
    {
        var id = Guid.NewGuid();
        var url = $"https://{_accessor.HttpContext?.Request.Host}/api/{id}";
        var attributes = new List<KeyValuePair<string, object?>> { new("url", url) };
        using var _ = _tracer.StartActiveSpan("Saving new request", initialAttributes: new SpanAttributes(attributes));
        
        _counter.Add(1, new KeyValuePair<string, object?>("url", url));

        var webhook = new Webhook
        {
            Id = id,
            Secret = request.Secret,
            Url = url,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.Add(webhook);

        return webhook.Url;
    }
}
