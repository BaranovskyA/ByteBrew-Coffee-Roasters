using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ByteBrew_Coffee_Roasters.Pages.Account
{
    [Authorize]
    public class LogoutModel(SignInManager<User> signInManager) : PageModel
    {
        public readonly SignInManager<User> _signInManager = signInManager;
        public async Task<IActionResult> OnGetAsync()
        {
            HttpContext.Session.Remove("CartId");
            await _signInManager.SignOutAsync();
            return RedirectPermanent("/Index");
        }
    }
}
