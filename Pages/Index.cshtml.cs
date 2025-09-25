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

    public List<string> HookIds { get; set; } = new();

    public void OnGet()
    {
        HookIds = _storage.GetAllIds();
    }

    public IActionResult OnPost()
    {
        var newId = _storage.GenerateId();
        _storage.InitializeId(newId);
        return RedirectToPage("/Index");
    }
}
