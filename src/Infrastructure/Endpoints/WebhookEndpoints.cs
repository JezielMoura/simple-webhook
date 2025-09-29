using Microsoft.AspNetCore.Http.HttpResults;
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

        route.MapGet("webhooks/{id:guid}", async (GetWebhookQuery query, Guid id, [FromHeader(Name = "Authorization")]string? secret) =>
            (await query.Execute(new GetWebhookRequest(id, secret))).Match<Results<Ok<GetWebhookResponse>, BadRequest<Error>>>(data => TypedResults.Ok(data), error => TypedResults.BadRequest(error)));

        route.MapDelete("webhooks/{id:guid}", async (DeleteWebhookCommand command, Guid id, [FromHeader(Name = "Authorization")]string? secret) =>
            (await command.Execute(new DeleteWebhookRequest(id, secret))).Match<Results<Ok<bool>, BadRequest<Error>>>(data => TypedResults.Ok(data), error => TypedResults.BadRequest(error)));
            
        route.MapMethods("{id:guid}", httpMethods, async (CreateRequestCommand command, Guid id, [FromHeader(Name = "Authorization")]string? secret) =>
            (await command.Execute(new CreateRequestRequest(id, secret))).Match<Results<Ok<Guid>, UnauthorizedHttpResult>>(data => TypedResults.Ok(data), error => TypedResults.Unauthorized()));
    }
}
