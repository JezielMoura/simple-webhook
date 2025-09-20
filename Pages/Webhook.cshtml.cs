using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Webhook.Services;

namespace Webhook.Pages
{
    public class WebhookModel : PageModel
    {
        private readonly WebhookService _webhookService;

        public WebhookModel(WebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; } = string.Empty;

        public List<Models.WebhookRequest> Requests { get; set; } = new();

        public string WebhookUrl => $"{Request.Scheme}://{Request.Host}/webhook/{Id}";

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Id) || !_webhookService.WebhookExists(Id))
            {
                return NotFound("Webhook not found");
            }

            Requests = _webhookService.GetRequests(Id);
            return Page();
        }

        public IActionResult OnPostClear()
        {
            if (string.IsNullOrEmpty(Id) || !_webhookService.WebhookExists(Id))
            {
                return NotFound("Webhook not found");
            }

            _webhookService.ClearRequests(Id);
            return RedirectToPage(new { id = Id });
        }

        public Dictionary<string, string> ParseHeaders(string? headersJson)
        {
            if (string.IsNullOrEmpty(headersJson))
                return new Dictionary<string, string>();

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(headersJson) 
                       ?? new Dictionary<string, string>();
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        public string FormatJson(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            try
            {
                using var doc = JsonDocument.Parse(json);
                return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                return json;
            }
        }
    }
}