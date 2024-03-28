using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Identity;
using ByteBrew_Coffee_Roasters.Data;
using ByteBrew_Coffee_Roasters.ViewModels;

namespace ByteBrew_Coffee_Roasters.Pages
{
    public class RegisterModel(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly List<Role> Roles = context.Roles.ToList();

        public IActionResult OnGet(string? Error)
        {
            ViewData["Roles"] = new SelectList(Roles, "Id", "Name");
            this.Error = Error;
            return Page();
        }

        [BindProperty]
        public RegisterViewModel ViewModel { get; set; } = default!;
        public string? Error { get; set; } = null;

        public async Task<IActionResult> OnPostAsync()
        {
            User user = new(
                ViewModel.RoleId,
                ViewModel.UserName
                );

            var result = await _userManager.CreateAsync(user, ViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToPage("./Index");
            }

            Error = "Произошла ошибка. Попробуйте позже";
            return RedirectToAction("Register", new { Error });
        }
    }
}
