namespace SimpleWebhook.Domain.RequestAggregate;

public class Request
{
    public Guid Id { get; set; }
    public Guid WebhookId { get; set; }
    public string Method { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = [];
    public string Body { get; set; } = string.Empty;
    public string QueryParameters { get; set; } = string.Empty;
    public string SourceIp { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}
