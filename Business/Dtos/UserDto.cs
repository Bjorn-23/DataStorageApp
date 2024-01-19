using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserDto
{

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
     
    public string UserRoleName { get; set; } = null!;

}
