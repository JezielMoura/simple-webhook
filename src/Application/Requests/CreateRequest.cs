using Microsoft.AspNetCore.Http;
using SimpleWebhook.Domain.RequestAggregate;

namespace SimpleWebhook.Application.Requests;

public sealed record CreateRequestRequest(Guid WebhookId);

public sealed class CreateRequestCommand(IHttpContextAccessor accessor, IRequestRepository repository)
{
    private readonly IHttpContextAccessor _accessor = accessor;
    private readonly IRequestRepository _repository = repository;
    private static JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public async Task<Guid> Execute(CreateRequestRequest request)
    {
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
            Body = JsonSerializer.Serialize(json),
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