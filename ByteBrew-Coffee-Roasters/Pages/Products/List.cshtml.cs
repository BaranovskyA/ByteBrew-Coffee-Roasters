using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ByteBrew_Coffee_Roasters.Data;
using ByteBrew_Coffee_Roasters.Data.Models;

namespace ByteBrew_Coffee_Roasters.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly ByteBrew_Coffee_Roasters.Data.ApplicationDbContext _context;

        public ListModel(ByteBrew_Coffee_Roasters.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Products.ToListAsync();
        }
    }
}
