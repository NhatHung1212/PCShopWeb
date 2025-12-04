using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;
using ShopWeb.Services;

namespace ShopWeb.Controllers;

public class ProductController : BaseController
{
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _context;

    public ProductController(IProductService productService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        : base(context, userManager)
    {
        _productService = productService;
        _context = context;
    }

    public async Task<IActionResult> Index(string searchTerm, long? categoryId)
    {
        var products = await _productService.GetAllProductsAsync();
        
        // Filter by category
        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }
        
        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            products = products.Where(p => 
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (p.Description != null && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
            ViewBag.SearchTerm = searchTerm;
        }

        // Load categories for filter
        ViewBag.Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
        ViewBag.SelectedCategoryId = categoryId;
        
        return View(products);
    }

    public async Task<IActionResult> Details(long id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        // Load reviews separately to avoid collation issues
        var reviews = await _context.Reviews
            .Where(r => r.ProductId == id)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        // Load users for reviews
        var userIds = reviews.Select(r => r.UserId).Distinct().ToList();
        var users = await _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        // Manually populate User navigation property
        foreach (var review in reviews)
        {
            review.User = users.FirstOrDefault(u => u.Id == review.UserId);
        }

        product.Reviews = reviews;

        return View(product);
    }
}
