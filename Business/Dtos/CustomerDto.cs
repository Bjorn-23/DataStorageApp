namespace Business.Dtos;

public class CustomerDto
{
    public Guid Id { get; protected private set; } = Guid.NewGuid(); //might need to be changed from protected private set.
    public string FirstName { get; set; } = null!;
      
    public string LastName { get; set; } = null!;
       
    public string EmailId { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}
