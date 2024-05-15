using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ByteBrew_Coffee_Roasters.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ByteBrew_Coffee_Roasters.Pages.Products
{
    public class ListModel(ByteBrew_Coffee_Roasters.Data.ApplicationDbContext context) : PageModel
    {
        private readonly ByteBrew_Coffee_Roasters.Data.ApplicationDbContext _context = context;

        public IList<Product> Products { get; set; } = context.Products.ToList();
        public const string CartSessionKey = "CartId";

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();

            var cartItems = GetCartItems();
            foreach (var product in Products)
            {
                var cartItem = cartItems.FirstOrDefault(x => x.ProductId == product.Id);

                if (cartItem == null)
                {
                    product.QuantityInCart = 0;
                }
                else
                {
                    product.QuantityInCart = cartItem.Quantity;
                }
            }
        }

        public async Task<RedirectResult> OnPostAddToCart(Guid productId, string returnUrl)
        {
            // ID корзины из браузера           
            Guid ShoppingCartId = GetCartId();

            var cartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync(
                c => c.CartId == ShoppingCartId
                && c.ProductId == productId);

            if (cartItem == null)
            {
                // Создаем новый               
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid(),
                    ProductId = productId,
                    CartId = ShoppingCartId,
                    Product = _context.Products.SingleOrDefault(p => p.Id == productId),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                await _context.ShoppingCartItems.AddAsync(cartItem);
            }
            else
            {
                // Если есть в корзине, то количество++                
                cartItem.Quantity++;
                Products.Single(x => x.Id == productId).QuantityInCart++;
            }
            await _context.SaveChangesAsync();
            return RedirectPermanent(returnUrl);
        }

        public async Task<RedirectResult> OnPostDecreaseFromCart(Guid productId, string returnUrl)
        {
            // ID корзины из браузера           
            Guid ShoppingCartId = GetCartId();

            var cartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync(
                c => c.CartId == ShoppingCartId
                && c.ProductId == productId);

            if (cartItem != null)
            {
                // Если остался один, то удаляем
                if (cartItem.Quantity == 1)
                {
                    _context.ShoppingCartItems.Remove(cartItem);
                }
                // Иначе отнимаем один из количества
                else
                {
                    cartItem.Quantity--;
                    Products.Single(x => x.Id == productId).QuantityInCart--;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectPermanent(returnUrl);
        }

        public async Task<RedirectResult> OnPostRemoveFromCart(Guid productId, string returnUrl)
        {
            // ID корзины из браузера        
            Guid ShoppingCartId = GetCartId();

            var cartItem = await _context.ShoppingCartItems
                .SingleOrDefaultAsync(x => x.CartId == ShoppingCartId && x.ProductId == productId);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectPermanent(returnUrl);
        }

        public Guid GetCartId()
        {
            if (HttpContext.Session.Get(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity!.Name))
                {
                    Guid userId = new (HttpContext.User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
                    HttpContext.Session.Set(CartSessionKey, userId.ToByteArray());
                }
                else
                {
                    // Анонимный гость
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Session.Set(CartSessionKey, tempCartId.ToByteArray());
                }
            }

            return new Guid(HttpContext.Session.Get(CartSessionKey)!);
        }

        public List<CartItem> GetCartItems()
        {
            Guid ShoppingCartId = GetCartId();

            return _context.ShoppingCartItems.Where(c => c.CartId == ShoppingCartId).ToList();
        }
    }
}
