using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class Customer_AddressesEntity
{
    public int Customer_AddressID { get; set; }

    [Key]
    [ForeignKey(nameof(AddressesEntity))]
    public int Addresses_Id { get; set; }

    [Key]
    [ForeignKey(nameof(CustomerEntity))]
    public int Customers_Id { get; set; }
}