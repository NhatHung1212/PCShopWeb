using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models;

public class Review
{
    public long Id { get; set; }
    
    [Required]
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [StringLength(1000)]
    public string? Comment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
