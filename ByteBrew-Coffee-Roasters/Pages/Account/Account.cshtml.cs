using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ByteBrew_Coffee_Roasters.Data;
using ByteBrew_Coffee_Roasters.Data.Models;

namespace ByteBrew_Coffee_Roasters.Pages.Account
{
    public class AccountModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AccountModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ByteBrew_Coffee_Roasters.Data.Models.User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users.FindAsync(User.Id);

            if (user == null) return NotFound();

            user.Email = User.Email;
            user.NormalizedEmail = User.Email?.ToUpper();
            user.PhoneNumber = User.PhoneNumber;
            user.PhoneNumberConfirmed = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
