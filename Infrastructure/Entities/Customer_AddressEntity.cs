using System.ComponentModel.DataAnnotations;


namespace Infrastructure.Entities;

public class Customer_AddressEntity
{
    [Required]
    public int AddressId { get; set; }
    public virtual AddressEntity Address { get; set; } = null!;

    [Required]
    public Guid CustomerId { get; set; }
    public virtual CustomerEntity Customer { get; set; } = null!;

}
