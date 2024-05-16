using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteBrew_Coffee_Roasters.Pages.Orders
{
    [Authorize(Roles = "Менеджер, Админ")]
    public class ListModel : PageModel
    {
        private readonly ByteBrew_Coffee_Roasters.Data.ApplicationDbContext _context;

        public ListModel(ByteBrew_Coffee_Roasters.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; } = new List<Order>();

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Where(x => x.Status == "Создан")
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .OrderBy(x => x.DateTime)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostOrderCompleteAsync(Guid orderId)
        {
            Order? order = await _context.Orders.FindAsync([orderId]);

            if (order != null)
            {
                order.Status = "Выполнен";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
