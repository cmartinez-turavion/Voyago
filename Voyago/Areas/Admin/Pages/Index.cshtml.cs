using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Voyago.Services;

namespace Voyago.Areas.Admin.Pages;

[Authorize(Policy = AuthorizationPolicies.AdminOnly)]
public sealed class IndexModel : PageModel
{
    public void OnGet()
    {
    }
}
