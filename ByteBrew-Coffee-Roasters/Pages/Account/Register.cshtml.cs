using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Identity;
using ByteBrew_Coffee_Roasters.ViewModels;
using ByteBrew_Coffee_Roasters.Data;

namespace ByteBrew_Coffee_Roasters.Pages.Account
{
    public class RegisterModel(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly List<Role> Roles = context.Roles.ToList();

        public IActionResult OnGet(string? Error, string? UserName)
        {
            ViewData["Roles"] = new SelectList(Roles, "Id", "Name");
            this.Error = Error;
            if (UserName != null)
                ViewModel.UserName = UserName;
            return Page();
        }

        [BindProperty]
        public RegisterViewModel ViewModel { get; set; } = new RegisterViewModel();
        public string? Error { get; set; } = null;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ViewModel.Password.Equals(ViewModel.ConfirmPassword))
            {
                Error = "Ошибка: Пароли не совпадают.";
                return RedirectToAction("Register", new { Error, ViewModel.UserName });
            }

            User user = new(
                ViewModel.RoleId,
                ViewModel.UserName
                );

            var result = await _userManager.CreateAsync(user, ViewModel.Password);
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(user.UserName!);

                IdentityUserRole<Guid> userRole = new IdentityUserRole<Guid>();
                userRole.UserId = currentUser!.Id;
                userRole.RoleId = currentUser.RoleId;

                context.UserRoles.Add(userRole);
                await context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToPage("/Index");
            }

            Error = "Ошибка: Попробуйте позже";
            return RedirectToAction("Register", new { Error });
        }
    }
}
