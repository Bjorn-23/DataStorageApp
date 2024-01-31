namespace Business.Dtos;

public class UserDto
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string SecurityKey { get; set; } = null!;

    public DateTime Created {  get; set; }

    public bool isActive { get; set; }

    public string UserRoleName { get; set; } = null!;

}
