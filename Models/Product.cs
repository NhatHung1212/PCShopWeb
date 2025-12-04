using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopWeb.Models;

public class Product
{
    public long Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Column("cost_price")]
    [Range(0.01, double.MaxValue)]
    public decimal CostPrice { get; set; } // Giá nhập

    [Required]
    [Column("stock")]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; } = 0; // Số lượng tồn kho

    [Column("image")]
    public string? ImageUrl { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    // Category relationship
    public long? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    // Reviews relationship
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
