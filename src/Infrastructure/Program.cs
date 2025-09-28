using Scalar.AspNetCore;
using SimpleWebhook.Infrastructure.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureEntityFramework(builder.Configuration);
builder.Services.ConfigureCommands();
builder.Services.AddHttpContextAccessor ();
builder.Services.AddInMemoryRepositories();
builder.Services.AddSingleton(_ => TimeProvider.System);
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "SimpleWebhook API",
            Version = "v1",
            Description = "API for store webhook requests."
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
}

app.MapOpenApi();
app.MapScalarApiReference("/docs");

app.MapGroup("api").WithTags("Webhooks").MapWebhookEndpoints();

await app.RunAsync();
