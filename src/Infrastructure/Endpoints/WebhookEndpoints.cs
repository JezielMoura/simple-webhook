using Microsoft.AspNetCore.Mvc;
using SimpleWebhook.Application.Requests;
using SimpleWebhook.Application.Webhooks;

namespace SimpleWebhook.Infrastructure.Endpoints;

public static class WebhookEndpoints
{
    private static readonly string[] httpMethods = ["GET", "POST", "PUT", "PATCH", "OPTIONS", "HEAD"];

    public static void MapWebhookEndpoints(this RouteGroupBuilder route)
    {
        route.MapPost("webhooks", async (CreateWebhookCommand command, CreateWebHookRequest request) =>
            TypedResults.Ok(await command.Execute(request)));

        route.MapGet("webhooks", async (ListWebhookQuery query) =>
            TypedResults.Ok(await query.Execute()));

        route.MapGet("webhooks/{id:guid}", async (GetWebhookQuery query, Guid id, [FromHeader]string? secret) =>
            TypedResults.Ok(await query.Execute(new GetWebhookRequest(id, secret))));

        route.MapDelete("webhooks/{id:guid}", async (DeleteWebhookCommand command, Guid id) =>
            TypedResults.Ok(await command.Execute(new DeleteWebhookRequest(id))));
            
        route.MapMethods("{id:guid}", httpMethods, async (CreateRequestCommand command, Guid id) =>
            TypedResults.Ok(await command.Execute(new CreateRequestRequest(id))));
    }
}
