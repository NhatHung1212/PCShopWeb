using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models;

public class Category
{
    public long Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string? IconClass { get; set; } // FontAwesome icon class
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
