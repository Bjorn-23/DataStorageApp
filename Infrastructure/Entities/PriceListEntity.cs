using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class PriceListEntity
{
    [Key]
    public string ArticleNumber { get; set; } = null!;
    public ProductEntity Product { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? DiscountPrice { get; set; }

    [Required]
    [StringLength(20)]
    public string UnitType { get; set; } = null!;
}