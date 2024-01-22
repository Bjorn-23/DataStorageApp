namespace Business.Dtos;

public class CustomerDetailsDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string UserRoleName { get; set; } = null!;
}
