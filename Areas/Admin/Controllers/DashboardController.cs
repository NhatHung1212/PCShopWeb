using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;
using ShopWeb.Services;

namespace ShopWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAnalyticsService _analyticsService;

    public DashboardController(
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager,
        IAnalyticsService analyticsService)
    {
        _context = context;
        _userManager = userManager;
        _analyticsService = analyticsService;
    }

    public async Task<IActionResult> Index()
    {
        // Analytics Data
        var totalRevenue = await _analyticsService.GetTotalRevenueAsync();
        var monthlyRevenue = await _analyticsService.GetMonthlyRevenueAsync();
        var totalOrders = await _analyticsService.GetTotalOrdersAsync();
        var pendingOrders = await _analyticsService.GetPendingOrdersAsync();
        var totalCustomers = await _analyticsService.GetTotalCustomersAsync();
        var totalProducts = await _analyticsService.GetTotalProductsAsync();
        
        // Charts Data
        var topProducts = await _analyticsService.GetTopSellingProductsAsync(5);
        var orderStatusDistribution = await _analyticsService.GetOrderStatusDistributionAsync();
        var revenueByMonth = await _analyticsService.GetRevenueByMonthAsync(6);
        var productsByCategory = await _analyticsService.GetProductsByCategoryAsync();
        var inventoryStatus = await _analyticsService.GetInventoryStatusAsync();
        var ordersByMonth = await _analyticsService.GetOrdersByMonthAsync(6);
        
        // Orders by Status (existing code)
        var processingOrders = await _context.Orders.CountAsync(o => o.Status == "Processing");
        var shippedOrders = await _context.Orders.CountAsync(o => o.Status == "Shipped");
        var deliveredOrders = await _context.Orders.CountAsync(o => o.Status == "Delivered");
        var cancelledOrders = await _context.Orders.CountAsync(o => o.Status == "Cancelled");
        var cancelRequests = await _context.Orders.CountAsync(o => o.CancelRequested && o.Status != "Cancelled");
        
        // Recent Orders
        var recentOrders = await _context.Orders
            .OrderByDescending(o => o.Id)
            .Take(10)
            .ToListAsync();
        
        // Load related data separately to avoid collation issues
        var orderIds = recentOrders.Select(o => o.Id).ToList();
        var orderDetails = await _context.OrderDetails
            .Include(od => od.Product)
            .Where(od => orderIds.Contains(od.OrderId))
            .ToListAsync();
        
        var userIds = recentOrders.Select(o => o.UserId).Distinct().ToList();
        var users = await _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
        
        // Manually populate navigation properties
        foreach (var order in recentOrders)
        {
            order.OrderDetails = orderDetails.Where(od => od.OrderId == order.Id).ToList();
            order.User = users.FirstOrDefault(u => u.Id == order.UserId);
        }

        // Analytics ViewBag
        ViewBag.TotalRevenue = totalRevenue;
        ViewBag.MonthlyRevenue = monthlyRevenue;
        ViewBag.TotalOrders = totalOrders;
        ViewBag.TotalCustomers = totalCustomers;
        ViewBag.TotalProducts = totalProducts;
        ViewBag.TopProducts = topProducts;
        ViewBag.OrderStatusDistribution = orderStatusDistribution;
        ViewBag.RevenueByMonth = revenueByMonth;
        ViewBag.ProductsByCategory = productsByCategory;
        ViewBag.InventoryStatus = inventoryStatus;
        ViewBag.OrdersByMonth = ordersByMonth;
        
        // Existing ViewBag
        ViewBag.PendingOrders = pendingOrders;
        ViewBag.ProcessingOrders = processingOrders;
        ViewBag.ShippedOrders = shippedOrders;
        ViewBag.DeliveredOrders = deliveredOrders;
        ViewBag.CancelledOrders = cancelledOrders;
        ViewBag.CancelRequests = cancelRequests;
        ViewBag.RecentOrders = recentOrders;

        return View();
    }
}
