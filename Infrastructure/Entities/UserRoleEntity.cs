using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class UserRoleEntity
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = null!;        

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
