﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class UserEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string Password { get; set; } = null!;

    [Required]
    [StringLength(200)]
    [Column(TypeName = "varchar")]
    public string SecurityKey { get; set; } = null!;

    [Required]
    public string UserRoleName { get; set; } = null!;
    public UserRoleEntity UserRole { get; set; } = null!;
}
