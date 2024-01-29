using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey("Id", "OrderId")]
[Index("ArticleNumber", Name = "UQ__OrderRow__3C991142E6BF2095", IsUnique = true)]
public partial class OrderRowEntity
{
    [Key]
    public int Id { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal OrderRowPrice { get; set; }

    public string ArticleNumber { get; set; } = null!;

    [Key]
    public int OrderId { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("OrderRow")]
    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderRows")]
    public virtual OrderEntity Order { get; set; } = null!;
}
