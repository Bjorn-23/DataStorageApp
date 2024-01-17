using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class AddressEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Country { get; set; } = null!;

    [Required]
    [StringLength(10)]
    [Column(TypeName = "varchar")]
    public string PostalCode { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string StreetName { get; set; } = null!;
}
