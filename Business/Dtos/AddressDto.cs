using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class AddressDto
{

    public int Id { get; set; }
 
    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string PostalCode { get; set; } = null!;
 
    public string StreetName { get; set; } = null!;
}
