using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Data;
using ShopWeb.Models;
using System.Security.Claims;

namespace ShopWeb.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReviewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(long productId, int rating, string comment)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // Check if user already reviewed this product
        var existingReview = _context.Reviews
            .FirstOrDefault(r => r.ProductId == productId && r.UserId == userId);

        if (existingReview != null)
        {
            // User already reviewed - don't allow duplicate
            TempData["Error"] = "Bạn đã đánh giá sản phẩm này rồi. Vui lòng sửa hoặc xóa đánh giá cũ.";
            return RedirectToAction("Details", "Product", new { area = "", id = productId });
        }

        // Create new review
        var review = new Review
        {
            ProductId = productId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đánh giá của bạn đã được gửi thành công!";
        return RedirectToAction("Details", "Product", new { area = "", id = productId });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, int rating, string comment, long productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var review = await _context.Reviews.FindAsync(id);

        if (review == null || review.UserId != userId)
        {
            TempData["Error"] = "Không tìm thấy đánh giá hoặc bạn không có quyền sửa.";
            return RedirectToAction("Details", "Product", new { area = "", id = productId });
        }

        review.Rating = rating;
        review.Comment = comment;
        review.CreatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Đánh giá đã được cập nhật!";
        return RedirectToAction("Details", "Product", new { area = "", id = productId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long id, long productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var review = await _context.Reviews.FindAsync(id);

        if (review == null || review.UserId != userId)
        {
            TempData["Error"] = "Không tìm thấy đánh giá hoặc bạn không có quyền xóa.";
            return RedirectToAction("Details", "Product", new { area = "", id = productId });
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đánh giá đã được xóa!";
        return RedirectToAction("Details", "Product", new { area = "", id = productId });
    }
}
