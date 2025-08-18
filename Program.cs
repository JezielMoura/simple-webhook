using System.Text.Json;

var builder = WebApplication.CreateSlimBuilder(args);
var callbacks = new List<Callback>();
var app = builder.Build();

JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

app.MapPost("/callbacks", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var content = await reader.ReadToEndAsync();
    using JsonDocument document = JsonDocument.Parse(content);
    var formattedJson = JsonSerializer.Serialize(document.RootElement, jsonSerializerOptions);
    var ip = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
    callbacks.Add(new(Guid.NewGuid(), formattedJson, ip, DateTime.UtcNow, false));
    return Results.Text("Ok");
});

app.MapGet("/callbacks", () =>
{
    if (callbacks.OrderBy(x => x.CreatedAt).FirstOrDefault(x => x.Sended == false) is { } callback)
    {
        callback.Sended = true;
        return Results.Ok(callback);
    }

    return Results.NotFound("Nenhum callback pendente");
});

app.MapGet("/", () =>
{
    var html = new StringBuilder("<html><head><title>Simple Webhook</title></head><body>");
    html.AppendLine("<style>html { font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; } </style>");
    html.AppendLine("<style>body { display: flex; line-height: 1.5; background-color: #1d1934; color: #f6f6f6; margin: 0; padding: 0; }</style>");
    html.AppendLine("<style> .enviado { background: #339900;} .pendente { background: #ff9966;} </style>");
    html.AppendLine("<div style='width: 300px; height: 100%; position: fixed; overflow: auto; border-right: 2px solid #2f2a6dff; background: #1d1934;'>");
    foreach (var callback in callbacks.OrderByDescending(x => x.CreatedAt))
    {
        var status = callback.Sended ? "enviado" : "pendente";
        html.AppendLine($"<div style='padding: 10px; margin: 15px 10px; cursor: pointer;' class='{status}' onclick='show(\"{callback.Id}\")'>");
        html.AppendLine($"{callback.Ip}<br/>{callback.CreatedAt.AddHours(-3).ToString("dd/MM/yyyy hh:mm:ss")}</div>");
    }
    html.AppendLine("</div><div style='width: calc(100% - 300px); padding-left: 300px'>");
    foreach (var callback in callbacks.OrderByDescending(x => x.CreatedAt))
    {
        html.AppendLine($"<div style='padding: 10px 20px; display: none' id={callback.Id} class='content'><pre><code>{callback.Content}</code></pre></div>");
    }
    html.AppendLine("<script>function show(id){Array.from(document.getElementsByClassName('content')).forEach(x => x.style.display = 'none'); document.getElementById(id).style.display = 'block'}</script>");
    html.AppendLine("</div></body></html>");
    return Results.Text(html.ToString(), "text/html", Encoding.UTF8);
});

app.Run();

class Callback(Guid id, string content, string ip, DateTime createdAt, bool sended)
{
    public Guid Id { get; } = id;
    public string Content { get; } = content;
    public string Ip { get; } = ip;
    public DateTime CreatedAt { get; } = createdAt;
    public bool Sended { get; set; } = sended;
}
