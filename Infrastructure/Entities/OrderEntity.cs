using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

public partial class OrderEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "money")]
    public decimal OrderPrice { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderRowEntity> OrderRows { get; set; } = new List<OrderRowEntity>();
}
