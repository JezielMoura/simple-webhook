using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webhook.Services;

namespace webhook.Pages;

public class IndexModel : PageModel
{
    private readonly IWebHookStorage _storage;

    public IndexModel(IWebHookStorage storage)
    {
        _storage = storage;
    }

    public string? HookId { get; set; }

    public void OnGet()
    {
        HookId = _storage.GenerateId();
    }
}
