using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Controllers;

[Authorize]
public class OrderController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        : base(context, userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        var user = await _userManager.GetUserAsync(User);
        ViewBag.User = user;

        // Compute subtotal and shipping (VND): add 20,000đ if subtotal < 500,000đ
        var subtotal = cartItems.Sum(ci => ci.Product!.Price * ci.Quantity);
        var shipping = subtotal < 500_000m ? 20_000m : 0m;
        ViewBag.Subtotal = subtotal;
        ViewBag.Shipping = shipping;
        ViewBag.Total = subtotal + shipping;

        return View(cartItems);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(string shippingAddress)
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        // Calculate subtotal and apply shipping rule: +20,000đ if subtotal < 500,000đ
        var subtotal = cartItems.Sum(ci => ci.Product!.Price * ci.Quantity);
        var shipping = subtotal < 500_000m ? 20_000m : 0m;

        var order = new Order
        {
            UserId = userId!,
            ShippingAddress = shippingAddress,
            TotalAmount = subtotal + shipping,
            Status = "Pending"
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Create order details and update stock
        foreach (var item in cartItems)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product!.Price
            };

            _context.OrderDetails.Add(orderDetail);
        }

        // Clear cart
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Order placed successfully!";
        return RedirectToAction("Details", new { id = order.Id, area = "" });
    }

    public async Task<IActionResult> Details(long id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound();
        }

        // Recompute subtotal and shipping from order details for display purposes
        var subtotal = order.OrderDetails.Sum(od => od.Price * od.Quantity);
        var shipping = Math.Max(0m, order.TotalAmount - subtotal);
        ViewBag.Subtotal = subtotal;
        ViewBag.Shipping = shipping;
        ViewBag.Total = order.TotalAmount;

        return View(order);
    }

    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.Id)
            .ToListAsync();

        return View(orders);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestCancel(long id, string cancelReason)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound();
        }

        // Only allow cancel request for Pending or Processing orders
        if (order.Status != "Pending" && order.Status != "Processing")
        {
            TempData["Error"] = "Cannot request cancellation for this order.";
            return RedirectToAction("Details", new { id, area = "" });
        }

        if (order.CancelRequested)
        {
            TempData["Error"] = "Cancel request already sent.";
            return RedirectToAction("Details", new { id, area = "" });
        }

        order.CancelRequested = true;
        order.CancelReason = cancelReason;
        await _context.SaveChangesAsync();

        TempData["Success"] = "Cancel request sent to admin. Please wait for confirmation.";
        return RedirectToAction("Details", new { id, area = "" });
    }
}
