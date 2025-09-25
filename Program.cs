
using webhook.Services;
using Microsoft.AspNetCore.Http;
using System.Text;
using webhook.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IWebHookStorage, WebHookStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapMethods("/hook/{id}", new[] { "GET", "POST", "PUT", "DELETE", "PATCH" }, async (HttpContext context, string id, IWebHookStorage storage) =>
{
    context.Request.EnableBuffering();
    var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    context.Request.Body.Position = 0;

    var log = new RequestLog
    {
        Method = context.Request.Method,
        Body = body,
        Timestamp = DateTime.UtcNow,
        IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        Headers = context.Request.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value.Select(v => v?.ToString() ?? "null")))
    };

    storage.LogRequest(id, log);

    return Results.Ok(new { status = "received", id });
});

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
