using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public partial class PriceListEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Column(TypeName = "money")]
    public decimal? DiscountPrice { get; set; }

    [StringLength(20)]
    public string UnitType { get; set; } = null!;

    [InverseProperty("Price")]
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
