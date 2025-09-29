namespace SimpleWebhook.Domain.WebhookAggregate;

public class Webhook
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
