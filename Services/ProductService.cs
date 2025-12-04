using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(long id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            // Kiểm tra xem sản phẩm có trong bất kỳ order nào không
            var hasOrders = await _context.OrderDetails
                .AnyAsync(od => od.ProductId == id);

            if (hasOrders)
            {
                throw new InvalidOperationException(
                    "Cannot delete this product. It exists in order history. " +
                    "Products that have been ordered cannot be deleted to maintain order records.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
