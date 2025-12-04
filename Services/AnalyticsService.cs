using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Services;

public interface IAnalyticsService
{
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetMonthlyRevenueAsync();
    Task<int> GetTotalOrdersAsync();
    Task<int> GetPendingOrdersAsync();
    Task<int> GetTotalCustomersAsync();
    Task<int> GetTotalProductsAsync();
    Task<List<ProductSalesDto>> GetTopSellingProductsAsync(int count = 5);
    Task<Dictionary<string, int>> GetOrderStatusDistributionAsync();
    Task<List<RevenueByMonthDto>> GetRevenueByMonthAsync(int months = 6);
    Task<Dictionary<string, int>> GetProductsByCategoryAsync();
    Task<Dictionary<string, int>> GetInventoryStatusAsync();
    Task<List<OrdersByMonthDto>> GetOrdersByMonthAsync(int months = 6);
}

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        // Revenue = (Selling Price - Cost Price) * Quantity for delivered orders
        var deliveredOrderIds = await _context.Orders
            .Where(o => o.Status == "Delivered")
            .Select(o => o.Id)
            .ToListAsync();
        
        var orderDetails = await _context.OrderDetails
            .Include(od => od.Product)
            .Where(od => deliveredOrderIds.Contains(od.OrderId) && od.Product != null)
            .ToListAsync();
        
        var profit = orderDetails.Sum(od => (od.Price - od.Product!.CostPrice) * od.Quantity);
        
        return profit;
    }

    public async Task<decimal> GetMonthlyRevenueAsync()
    {
        // Since Order doesn't have OrderDate, return total profit for now
        return await GetTotalRevenueAsync();
    }

    public async Task<int> GetTotalOrdersAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public async Task<int> GetPendingOrdersAsync()
    {
        return await _context.Orders
            .Where(o => o.Status == "Pending")
            .CountAsync();
    }

    public async Task<int> GetTotalCustomersAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<List<ProductSalesDto>> GetTopSellingProductsAsync(int count = 5)
    {
        var grouped = await _context.OrderDetails
            .Include(od => od.Product)
            .Where(od => od.Product != null)
            .GroupBy(od => new { od.ProductId, od.Product!.Name, od.Product.ImageUrl, od.Product.CostPrice })
            .ToListAsync();
        
        return grouped
            .Select(g => new ProductSalesDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                ImageUrl = g.Key.ImageUrl,
                TotalQuantity = g.Sum(od => od.Quantity),
                TotalRevenue = g.Sum(od => (od.Price - g.Key.CostPrice) * od.Quantity) // Profit
            })
            .OrderByDescending(p => p.TotalQuantity)
            .Take(count)
            .ToList();
    }

    public async Task<Dictionary<string, int>> GetOrderStatusDistributionAsync()
    {
        return await _context.Orders
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);
    }

    public async Task<List<RevenueByMonthDto>> GetRevenueByMonthAsync(int months = 6)
    {
        // Since Order doesn't have OrderDate, return sample data
        // You should add CreatedAt field to Order model for accurate tracking
        var currentMonth = DateTime.Now;
        var result = new List<RevenueByMonthDto>();
        var totalRevenue = await GetTotalRevenueAsync(); // Use profit calculation
        
        for (int i = months - 1; i >= 0; i--)
        {
            var month = currentMonth.AddMonths(-i);
            result.Add(new RevenueByMonthDto
            {
                Month = month.ToString("MMM yyyy"),
                Revenue = totalRevenue / months // Distribute evenly for demo
            });
        }
        
        return result;
    }

    public async Task<Dictionary<string, int>> GetProductsByCategoryAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .GroupBy(p => p.Category != null ? p.Category.Name : "Uncategorized")
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Category, x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetInventoryStatusAsync()
    {
        var products = await _context.Products.ToListAsync();
        return new Dictionary<string, int>
        {
            { "In Stock", products.Count(p => p.Stock >= 10) },
            { "Low Stock", products.Count(p => p.Stock > 0 && p.Stock < 10) },
            { "Out of Stock", products.Count(p => p.Stock == 0) }
        };
    }

    public async Task<List<OrdersByMonthDto>> GetOrdersByMonthAsync(int months = 6)
    {
        // Since Order doesn't have OrderDate, show all orders in current month
        var currentMonth = DateTime.Now;
        var result = new List<OrdersByMonthDto>();
        var totalOrders = await _context.Orders.CountAsync();
        
        for (int i = months - 1; i >= 0; i--)
        {
            var month = currentMonth.AddMonths(-i);
            result.Add(new OrdersByMonthDto
            {
                Month = month.ToString("MMM yyyy"),
                OrderCount = i == 0 ? totalOrders : 0 // All orders in current month
            });
        }
        
        return result;
    }
}

public class ProductSalesDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class RevenueByMonthDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class OrdersByMonthDto
{
    public string Month { get; set; } = string.Empty;
    public int OrderCount { get; set; }
}
