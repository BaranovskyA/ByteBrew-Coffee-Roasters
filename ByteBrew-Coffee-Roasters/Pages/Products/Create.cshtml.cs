using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace ByteBrew_Coffee_Roasters.Pages.Products
{
    [Authorize(Roles = "Менеджер, Админ")]
    public class CreateModel : PageModel
    {
        private readonly ByteBrew_Coffee_Roasters.Data.ApplicationDbContext _context;

        public CreateModel(ByteBrew_Coffee_Roasters.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
