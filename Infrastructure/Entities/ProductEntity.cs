using System.ComponentModel.DataAnnotations;


namespace Infrastructure.Entities;

public class ProductEntity
{
    [Key]
    public string ArticleNumber { get; set; } = null!;
    public PriceListEntity PriceList { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Title { get; set; } = null!;

    [StringLength(200)]
    public string? Ingress { get; set; } = null!;

    public string? Description {  get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;
}
