using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [StringLength(30)]
    public string Unit { get; set; } = null!;

    public int Stock { get; set; }

    public int PriceId { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual CategoryEntity Category { get; set; } = null!;

    [InverseProperty("ArticleNumberNavigation")]
    public virtual OrderRowEntity? OrderRow { get; set; }

    [ForeignKey("PriceId")]
    [InverseProperty("Products")]
    public virtual PriceListEntity Price { get; set; } = null!;
}
