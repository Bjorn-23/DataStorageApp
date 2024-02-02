﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[Index("CategoryName", Name = "UQ__Categori__8517B2E0773B58B3", IsUnique = true)]
public partial class CategoryEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
