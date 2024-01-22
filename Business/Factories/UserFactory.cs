using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class UserFactory
{
    public static UserEntity Create(UserDto dto)
    {
        try
        {
            return new UserEntity
            {
                Id = dto.Id,
                Email = dto.Email,
                Password = dto.Password,
                SecurityKey = dto.SecurityKey,
                UserRoleName = dto.UserRoleName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static UserDto Create(UserEntity entity)
    {
        try
        {
            return new UserDto
            {
                Id = entity.Id,
                Email = entity.Email,
                Password = entity.Password,
                SecurityKey = entity.SecurityKey,
                UserRoleName = entity.UserRoleName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
