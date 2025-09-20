using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text;
using System.Text.Json;
using Webhook.Models;
using Webhook.Services;

namespace Webhook.Pages
{
    public class WebhookReceiverModel : PageModel
    {
        private readonly WebhookService _webhookService;

        public WebhookReceiverModel(WebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !_webhookService.WebhookExists(id))
            {
                return NotFound("Webhook not found");
            }

            try
            {
                var request = HttpContext.Request;
                
                // Read the body
                string body = string.Empty;
                if (request.ContentLength > 0)
                {
                    using (var reader = new StreamReader(request.Body))
                    {
                        body = await reader.ReadToEndAsync();
                    }
                }

                // Get headers
                var headers = new Dictionary<string, string>();
                foreach (var header in request.Headers)
                {
                    headers[header.Key] = header.Value.ToString();
                }

                var webhookRequest = new WebhookRequest
                {
                    Id = Guid.NewGuid(),
                    WebhookId = id,
                    Method = request.Method,
                    Path = request.Path.Value,
                    QueryString = request.QueryString.Value,
                    Headers = JsonSerializer.Serialize(headers),
                    Body = body,
                    Timestamp = DateTime.UtcNow,
                    ContentType = request.ContentType,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = request.Headers["User-Agent"].ToString()
                };

                _webhookService.AddRequest(id, webhookRequest);

                // Return a success response
                return new JsonResult(new { success = true, message = "Webhook received successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || !_webhookService.WebhookExists(id))
            {
                return NotFound("Webhook not found");
            }

            try
            {
                var request = HttpContext.Request;
                
                // Get headers
                var headers = new Dictionary<string, string>();
                foreach (var header in request.Headers)
                {
                    headers[header.Key] = header.Value.ToString();
                }

                var webhookRequest = new WebhookRequest
                {
                    Id = Guid.NewGuid(),
                    WebhookId = id,
                    Method = request.Method,
                    Path = request.Path.Value,
                    QueryString = request.QueryString.Value,
                    Headers = JsonSerializer.Serialize(headers),
                    Body = string.Empty,
                    Timestamp = DateTime.UtcNow,
                    ContentType = request.ContentType,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = request.Headers["User-Agent"].ToString()
                };

                _webhookService.AddRequest(id, webhookRequest);

                // Return a success response
                return new JsonResult(new { success = true, message = "Webhook received successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        public async Task<IActionResult> OnPutAsync(string id)
        {
            return await OnPostAsync(id);
        }

        public async Task<IActionResult> OnPatchAsync(string id)
        {
            return await OnPostAsync(id);
        }

        public async Task<IActionResult> OnDeleteAsync(string id)
        {
            return await OnPostAsync(id);
        }

        public async Task<IActionResult> OnHeadAsync(string id)
        {
            return await OnGetAsync(id);
        }

        public async Task<IActionResult> OnOptionsAsync(string id)
        {
            return await OnGetAsync(id);
        }
    }
}