using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserDto
{

    public string EmailId { get; set; } = null!;

    public string PasswordId { get; set; } = null!;
     
    public string UserRoleName { get; set; } = null!;
!;
}
