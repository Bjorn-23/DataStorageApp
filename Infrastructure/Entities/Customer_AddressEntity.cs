using System.ComponentModel.DataAnnotations;


namespace Infrastructure.Entities;

public class Customer_AddressEntity
{
    [Required]
    public int AddressId { get; set; }
    public virtual AddressEntity Address { get; set; } = null!;

    [Required]
    public string CustomerId { get; set; } = null!;
    public virtual CustomerEntity Customer { get; set; } = null!;
}
