using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class UserEntity
{
    /// <summary>
    /// Generated upon first creation of a user
    /// </summary>
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Generated from UserRegistarionDto
    /// </summary>
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Generated from PasswordGenerator class
    /// </summary>
    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Generated from PasswordGenerator class
    /// </summary>
    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string SecurityKey { get; set; } = null!;

    [Required]
    public DateTime Created {  get; set; } = DateTime.Now;

    [Required]
    public bool IsActive { get; set; } = false;

    /// <summary>
    /// Generated from UserRoleServices upon creating new User. Alternatively added later - currently no method for that.
    /// </summary>
    [Required]
    public string UserRoleName { get; set; } = null!;
    public UserRoleEntity UserRole { get; set; } = null!;

}
