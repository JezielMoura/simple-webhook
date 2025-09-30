using SimpleWebhook.Infrastructure.Persistence.Contexts;
using SimpleWebhook.Domain.WebhookAggregate;
using SimpleWebhook.Domain.RequestAggregate;
using SimpleWebhook.Infrastructure.Persistence.Repositories;
using SimpleWebhook.Application.Webhooks;
using SimpleWebhook.Application.Requests;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using System.Diagnostics.Metrics;

namespace SimpleWebhook.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ServiceName = "Webhook";

    public static void ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(sp => new AppDbContext(configuration));
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
    }

    public static void AddInMemoryRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IWebhookRepository, InMemoryWebhookRepository>();
        services.AddSingleton<IRequestRepository, InMemoryRequestRepository>();
    }

    public static void ConfigureCommands(this IServiceCollection services)
    {
        services.AddScoped<CreateWebhookCommand>();
        services.AddScoped<CreateRequestCommand>();
        services.AddScoped<DeleteWebhookCommand>();
        services.AddScoped<GetWebhookQuery>();
        services.AddScoped<ListWebhookQuery>();
    }

    public static void ConfigureTelemetry(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddSingleton(TracerProvider.Default.GetTracer(ServiceName));
        services.AddSingleton(new Meter(ServiceName));

        services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder => resourceBuilder
                .AddService(ServiceName)
                .AddAttributes([new("deployment.environment.name", environment.EnvironmentName)]))
            .WithMetrics(meterBuilder => meterBuilder
                .AddMeter(ServiceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter())
            .WithTracing(tracerBuilder => tracerBuilder
                .AddSource(ServiceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter())
            .WithLogging(builder => builder.AddOtlpExporter(), options => (options.IncludeScopes, options.IncludeFormattedMessage) = (true, true));
    }
}
