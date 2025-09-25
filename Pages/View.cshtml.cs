using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webhook.Models;
using webhook.Services;

namespace webhook.Pages;

public class ViewModel : PageModel
{
    private readonly IWebHookStorage _storage;

    public ViewModel(IWebHookStorage storage)
    {
        _storage = storage;
    }

    public string Id { get; set; } = string.Empty;
    public List<RequestLog> Requests { get; set; } = new();

    public void OnGet(string id)
    {
        Id = id;
        Requests = _storage.GetRequests(id);
    }
}