using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[Index(nameof(Email), IsUnique = true)]
public class CustomerEntity
{
    [Key]
    public int CustomerId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Firstname { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Lastname { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

}



// [ForeignKey(nameof(ProductEntity))]
// public virtual ProductEntity Product { get; set; } = null!;


