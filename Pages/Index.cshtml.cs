using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webhook.Services;

namespace Webhook.Pages;

public class IndexModel : PageModel
{
    private readonly WebhookService _webhookService;

    public IndexModel(WebhookService webhookService)
    {
        _webhookService = webhookService;
    }

    [BindProperty(SupportsGet = true)]
    public string? WebhookId { get; set; }

    public string? NewWebhookUrl { get; set; }
    public List<string> AllWebhooks { get; set; } = new();

    public IActionResult OnGet()
    {
        AllWebhooks = _webhookService.GetAllWebhookIds();
        return Page();
    }

    public IActionResult OnPostCreateWebhook()
    {
        var webhookId = _webhookService.CreateWebhookUrl();
        return RedirectToPage(new { webhookId = webhookId });
    }

    public IActionResult OnPostDeleteWebhook(string id)
    {
        _webhookService.DeleteWebhook(id);
        return RedirectToPage();
    }
}
