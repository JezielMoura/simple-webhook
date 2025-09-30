using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;
using SimpleWebhook.Domain.RequestAggregate;
using SimpleWebhook.Domain.WebhookAggregate;

namespace SimpleWebhook.Application.Requests;

public sealed record CreateRequestRequest(Guid WebhookId, string? Secret);

public sealed class CreateRequestCommand(IHttpContextAccessor accessor, IRequestRepository repository, IWebhookRepository webhookRepository, Tracer tracer, Meter meter)
{
    private readonly IHttpContextAccessor _accessor = accessor;
    private readonly IRequestRepository _repository = repository;
    private readonly IWebhookRepository _webhookRepository = webhookRepository;
    private readonly Tracer _tracer = tracer;
    private readonly Counter<int> _counter = meter.CreateCounter<int>("webhook.requests");
    private static JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public async Task<Result<Guid, Error>> Execute(CreateRequestRequest request)
    {
        var webhook = await _webhookRepository.Find(request.WebhookId);

        if (webhook is null || (!string.IsNullOrWhiteSpace(webhook.Secret) && request.Secret != webhook.Secret))
        {
            return new Error { Message = "Unauthorized" };
        }

        _counter.Add(1, new KeyValuePair<string, object?>("url", webhook.Url));

        var attributes = new List<KeyValuePair<string, object?>> { new ("url", webhook.Url)};
        using var _ = _tracer.StartActiveSpan("Saving new request", initialAttributes: new SpanAttributes(attributes));

        var httpRequest = _accessor.HttpContext?.Request ?? throw new InvalidOperationException("Null HTTP request");
        var id = Guid.NewGuid();
        var body = await new StreamReader(httpRequest.Body).ReadToEndAsync();
        var json = TryConvertJson(body, out var output) ? output : "";
        var req = new Request
        {
            Id = id,
            WebhookId = request.WebhookId,
            Method = httpRequest.Method,
            Headers = httpRequest.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
            Body = JsonSerializer.Serialize(json, _jsonOptions),
            QueryParameters = string.Join(" | ", httpRequest.Query.Select(q => $"{q.Key}={q.Value}")),
            SourceIp = _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
            ReceivedAt = DateTime.UtcNow
        };

        await _repository.Add(req);

        return id;
    }

    public static bool TryConvertJson(string input, out object? output)
    {
        try
        {
            output = JsonSerializer.Deserialize<object>(input, _jsonOptions);
            return true;
        }
        catch
        {
            output = null;
            return false;
        }
    }
}