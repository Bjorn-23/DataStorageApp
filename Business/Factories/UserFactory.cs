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
                Email = dto.Email,
                Password = dto.Password,
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
                Email = entity.Email,
                Password = entity.Password,
                UserRoleName = entity.UserRoleName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
