using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CustomerEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Password { get; set; } = null!;

    [StringLength(16)]
    [Column(TypeName = "varchar")]
    public string PhoneNumber { get; set; } = null!;

}