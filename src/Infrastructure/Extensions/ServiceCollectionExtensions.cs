using SimpleWebhook.Infrastructure.Persistence.Contexts;
using SimpleWebhook.Domain.WebhookAggregate;
using SimpleWebhook.Domain.RequestAggregate;
using SimpleWebhook.Infrastructure.Persistence.Repositories;
using SimpleWebhook.Application.Webhooks;
using SimpleWebhook.Application.Requests;

namespace SimpleWebhook.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
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
}
