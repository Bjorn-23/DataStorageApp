using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CustomerEntity
{
    [Key]
    public string Id { get; set; } = null!;
  
    public UserEntity User { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string EmailId { get; set; } = null!;

    [StringLength(16)]
    [Column(TypeName = "varchar")]
    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Customer_AddressEntity> CustomerAddresses { get; set; } = new List<Customer_AddressEntity>();
}