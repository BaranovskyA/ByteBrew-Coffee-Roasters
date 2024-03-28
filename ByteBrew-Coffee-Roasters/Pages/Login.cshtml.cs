using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Identity;
using ByteBrew_Coffee_Roasters.ViewModels;

namespace ByteBrew_Coffee_Roasters.Pages
{
    public class LoginModel(UserManager<User> userManager, SignInManager<User> signInManager) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;

        public IActionResult OnGet(string? Error)
        {
            this.Error = Error;
            return Page();
        }

        [BindProperty]
        public LoginViewModel ViewModel { get; set; } = default!;
        public string? Error { get; set; } = null;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(ViewModel.UserName))
            {
                var user = await _userManager.FindByNameAsync(ViewModel.UserName);

                if (user != null && !string.IsNullOrEmpty(ViewModel.Password)) 
                {
                    await _signInManager.PasswordSignInAsync(user, ViewModel.Password, 
                        isPersistent: true, lockoutOnFailure: false);

                    return RedirectToPage("./Index");
                }
            }

            Error = "Неверно введено имя пользователя или пароль";
            return RedirectToAction("Login", new { Error });
        }
    }
}
