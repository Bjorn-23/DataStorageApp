using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Infrastructure.Entities;

public partial class ProductEntity
{
    [Key]
    public string ArticleNumber { get; set; } = null!;

    [StringLength(50)]
    public string Title { get; set; } = null!;

    [StringLength(200)]
    public string? Ingress { get; set; }

    public string? Description { get; set; }

    public int PriceId { get; set; }

    [StringLength(30)]
    public string Unit { get; set; } = null!;

    public int Stock { get; set; }

    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [ForeignKey("CategoryName")]
    [InverseProperty("Products")]
    public virtual CategoryEntity CategoryNameNavigation { get; set; } = null!;

    [InverseProperty("ArticleNumberNavigation")]
    public virtual OrderRowEntity? OrderRow { get; set; }

    [ForeignKey("PriceId")]
    [InverseProperty("Products")]
    public virtual PriceListEntity Price { get; set; } = null!;
}
