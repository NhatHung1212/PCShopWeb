using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Controllers;

[Authorize]
public class CartController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        : base(context, userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        // Calculate shipping: add 20,000đ if subtotal < 500,000đ
        var subtotal = cartItems.Sum(ci => ci.Product!.Price * ci.Quantity);
        var shipping = subtotal < 500_000m ? 20_000m : 0m;
        ViewBag.Subtotal = subtotal;
        ViewBag.Shipping = shipping;
        ViewBag.Total = subtotal + shipping;

        return View(cartItems);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(long productId, int quantity = 1)
    {
        var userId = _userManager.GetUserId(User);
        var product = await _context.Products.FindAsync(productId);

        if (product == null)
        {
            TempData["Error"] = "Product not found";
            return RedirectToAction("Index", "Product", new { area = "" });
        }

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                UserId = userId!,
                ProductId = productId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Product added to cart";

        return RedirectToAction("Index", new { area = "" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(long id, int quantity)
    {
        var userId = _userManager.GetUserId(User);
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);

        if (cartItem != null && quantity > 0)
        {
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { area = "" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(long id)
    {
        var userId = _userManager.GetUserId(User);
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);

        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { area = "" });
    }
}
