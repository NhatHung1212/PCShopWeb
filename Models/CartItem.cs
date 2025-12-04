using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopWeb.Models;

public class CartItem
{
    public long Id { get; set; }

    [Required]
    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    public virtual ApplicationUser? User { get; set; }

    [Required]
    [Column("product_id")]
    public long ProductId { get; set; }

    public virtual Product? Product { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
