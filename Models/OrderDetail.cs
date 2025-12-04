using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopWeb.Models;

public class OrderDetail
{
    public long Id { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    public virtual Order? Order { get; set; }

    [Required]
    [Column("product_id")]
    public long ProductId { get; set; }

    public virtual Product? Product { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public decimal Price { get; set; }
}
