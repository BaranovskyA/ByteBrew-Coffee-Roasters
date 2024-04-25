using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace ByteBrew_Coffee_Roasters.Pages.Products
{
    [Authorize(Roles = "Менеджер, Админ")]
    public class DeleteModel : PageModel
    {
        private readonly ByteBrew_Coffee_Roasters.Data.ApplicationDbContext _context;

        public DeleteModel(ByteBrew_Coffee_Roasters.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                Product = product;
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
