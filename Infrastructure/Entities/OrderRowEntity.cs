using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey("ArticleNumber", "OrderId")]
public partial class OrderRowEntity
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal OrderRowPrice { get; set; }

    [Key]
    [StringLength(100)]
    public string ArticleNumber { get; set; } = null!;

    [Key]
    public int OrderId { get; set; }

    [ForeignKey("ArticleNumber")]
    [InverseProperty("OrderRows")]
    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderRows")]
    public virtual OrderEntity Order { get; set; } = null!;
}
