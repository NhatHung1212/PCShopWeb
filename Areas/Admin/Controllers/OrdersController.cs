using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderDetails)
            .OrderByDescending(o => o.Id)
            .ToListAsync();

        // Load users separately to avoid collation conflict
        var userIds = orders.Select(o => o.UserId).Distinct().ToList();
        var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        
        foreach (var order in orders)
        {
            order.User = users.FirstOrDefault(u => u.Id == order.UserId);
        }

        return View(orders);
    }

    public async Task<IActionResult> Details(long id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        // Load user separately to avoid collation conflict
        order.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(long id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        order.Status = status;
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Order status updated to {status}";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveCancellation(long id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        if (!order.CancelRequested)
        {
            TempData["Error"] = "No cancellation request for this order";
            return RedirectToAction(nameof(Details), new { id });
        }

        order.Status = "Cancelled";
        order.CancelRequested = false; // Clear the request flag after processing
        await _context.SaveChangesAsync();

        TempData["Success"] = "Order has been cancelled";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectCancellation(long id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        if (!order.CancelRequested)
        {
            TempData["Error"] = "No cancellation request for this order";
            return RedirectToAction(nameof(Details), new { id });
        }

        order.CancelRequested = false;
        order.CancelReason = null;
        await _context.SaveChangesAsync();

        TempData["Success"] = "Cancellation request rejected";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        _context.OrderDetails.RemoveRange(order.OrderDetails);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Order deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
