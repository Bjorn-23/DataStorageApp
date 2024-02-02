using Infrastructure.Entities;

namespace Business.Dtos;

public class Customer_AddressDto
{
    public int AddressId { get; set; }
    public virtual AddressEntity Address { get; set; } = null!;

    public string CustomerId { get; set; } = null!;
    public virtual CustomerEntity Customer { get; set; } = null!;

}
