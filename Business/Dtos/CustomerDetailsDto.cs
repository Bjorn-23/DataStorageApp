namespace Business.Dtos;

public class CustomerDetailsDto
{
    public string Id { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string UserRoleName { get; set; } = null!;
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

}
