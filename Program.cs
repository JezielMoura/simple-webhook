using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
var callbacks = new List<Callback>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapPost("/callbacks", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var content = await reader.ReadToEndAsync();
    callbacks.Add(new(Guid.NewGuid(), content, DateTime.UtcNow, false));
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
    var html = new StringBuilder("<html><head><title>Callbacks</title></head><body>");
    html.AppendLine("<style>html {font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;}</style>");
    html.AppendLine("<style>body {display: flex; line-height: 1.5; background-color: #1d1934; color: #f6f6f6; margin: 0; padding: 0 }</style>");
    html.AppendLine("<style> .enviado { background: #00be36ff;} .pendente { background: #bd9700ff;} </style>");
    html.AppendLine("<div style='width:20%; height: 100%; position: fixed; overflow: auto; border-right: 2px solid #2f2a6dff;'>");
    foreach (var callback in callbacks.OrderByDescending(x => x.CreatedAt))
    {
        var status = callback.Sended ? "enviado" : "pendente";
        html.AppendLine($"<div style='padding: 10px; margin: 15px 10px; cursor: pointer;' class='{status}' onclick='show(\"{callback.Id}\")'>{callback.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss")} ({status})</div>");
    }
    html.AppendLine("</div><div style='width:80%; padding-left: 20%'>");
    foreach (var callback in callbacks.OrderByDescending(x => x.CreatedAt))
    {
        html.AppendLine($"<div style='padding: 10px 20px; display: none' id={callback.Id} class='content'><pre><code>{callback.Content}</code></pre></div>");
    }
    html.AppendLine("<script>function show(id){Array.from(document.getElementsByClassName('content')).forEach(x => x.style.display = 'none'); document.getElementById(id).style.display = 'block'}</script>");
    html.AppendLine("</div></body></html>");
    return Results.Text(html.ToString(), "text/html", Encoding.UTF8);
});

app.Run();

class Callback(Guid id, string content, DateTime createdAt, bool sended)
{
    public Guid Id { get; } = id;
    public string Content { get; } = content;
    public DateTime CreatedAt { get; } = createdAt;
    public bool Sended { get; set; } = sended;
}

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(Callback))]
[JsonSerializable(typeof(Callback[]))]
[JsonSerializable(typeof(List<Callback>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
