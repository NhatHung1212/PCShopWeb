using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopWeb.Models;

public class Order
{
    public long Id { get; set; }

    [Required]
    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    public virtual ApplicationUser? User { get; set; }

    [Required]
    [Column("total")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    [Column("address")]
    public string? ShippingAddress { get; set; }

    [Column("cancel_requested")]
    public bool CancelRequested { get; set; } = false;

    [Column("cancel_reason")]
    public string? CancelReason { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
