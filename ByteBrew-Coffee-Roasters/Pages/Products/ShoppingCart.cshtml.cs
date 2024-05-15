using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ByteBrew_Coffee_Roasters.Pages.Products
{
    public class ShoppingCartModel : PageModel
    {
        private readonly ByteBrew_Coffee_Roasters.Data.ApplicationDbContext _context;

        public ShoppingCartModel(ByteBrew_Coffee_Roasters.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();
        public const string CartSessionKey = "CartId";
        [BindProperty]
        public string StatusMessage { get; private set; }

        public async Task OnGetAsync()
        {
            CartItems = await _context.ShoppingCartItems
                .Where(x => x.CartId == GetCartId())
                .Include(c => c.Product)
                .ToListAsync();
        }

        public Guid GetCartId()
        {
            if (HttpContext.Session.Get(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity!.Name))
                {
                    Guid userId = new(HttpContext.User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
                    HttpContext.Session.Set(CartSessionKey, userId.ToByteArray());
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Session.Set(CartSessionKey, tempCartId.ToByteArray());
                }
            }

            return new Guid(HttpContext.Session.Get(CartSessionKey)!);
        }

        public async Task<IActionResult> OnPostAddToCart(Guid productId)
        {
            Guid ShoppingCartId = GetCartId();

            var cartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync(
                c => c.CartId == ShoppingCartId && c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid(),
                    ProductId = productId,
                    CartId = ShoppingCartId,
                    Product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                await _context.ShoppingCartItems.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDecreaseFromCart(Guid productId)
        {
            Guid ShoppingCartId = GetCartId();

            var cartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync(
                c => c.CartId == ShoppingCartId && c.ProductId == productId);

            if (cartItem != null)
            {
                if (cartItem.Quantity == 1)
                {
                    _context.ShoppingCartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity--;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromCart(Guid productId)
        {
            Guid ShoppingCartId = GetCartId();

            var cartItem = await _context.ShoppingCartItems
                .SingleOrDefaultAsync(x => x.CartId == ShoppingCartId && x.ProductId == productId);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMakeOrder()
        {
            Guid ShoppingCartId = GetCartId();

            var cartItems = await _context.ShoppingCartItems
                .Where(x => x.CartId == ShoppingCartId).ToListAsync();

            if (cartItems != null)
            {
                _context.ShoppingCartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }

            StatusMessage = "Успешно";
            return RedirectToPage();
        }
    }
}
