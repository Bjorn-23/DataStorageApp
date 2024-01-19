namespace Business.Dtos;

public class CustomerDetailsDto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string UserRoleName { get; set; } = null!;
}
