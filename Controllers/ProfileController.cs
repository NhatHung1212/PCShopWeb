using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Controllers;

[Authorize]
public class ProfileController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public ProfileController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context) : base(context, userManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get order statistics
        var orders = await _context.Orders
            .Where(o => o.UserId == user.Id)
            .ToListAsync();

        ViewBag.TotalOrders = orders.Count;
        ViewBag.TotalSpent = orders.Sum(o => o.TotalAmount);
        ViewBag.PendingOrders = orders.Count(o => o.Status == "Pending");
        ViewBag.ProcessingOrders = orders.Count(o => o.Status == "Processing");
        ViewBag.ShippedOrders = orders.Count(o => o.Status == "Shipped");
        ViewBag.DeliveredOrders = orders.Count(o => o.Status == "Delivered");
        ViewBag.CancelledOrders = orders.Count(o => o.Status == "Cancelled");

        return View(user);
    }

    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string fullName, string address, string phoneNumber)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        user.FullName = fullName;
        user.Address = address;
        user.PhoneNumber = phoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Index), new { area = "" });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount(string password)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Verify password
        var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordCheck)
        {
            TempData["Error"] = "Incorrect password. Account deletion cancelled.";
            return RedirectToAction(nameof(Index), new { area = "" });
        }

        // Delete user's orders and cart items first
        var orders = await _context.Orders.Where(o => o.UserId == user.Id).ToListAsync();
        _context.Orders.RemoveRange(orders);

        var cartItems = await _context.CartItems.Where(c => c.UserId == user.Id).ToListAsync();
        _context.CartItems.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        // Delete user account
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Your account has been deleted successfully.";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        TempData["Error"] = "Failed to delete account. Please try again.";
        return RedirectToAction(nameof(Index), new { area = "" });
    }
}
