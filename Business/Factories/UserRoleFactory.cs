using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class UserRoleFactory
{
    public static UserRoleEntity Create(UserRoleDto dto)
    {
        try
        {
            return new UserRoleEntity
            {
                RoleName = dto.RoleName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static UserRoleEntity Create(UserDto dto)
    {
        try
        {
            return new UserRoleEntity
            {
                RoleName = dto.UserRoleName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static UserRoleEntity Create(UserEntity entity)
    {
        try
        {
            return new UserRoleEntity
            {
                RoleName = entity.UserRoleName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static UserRoleDto Create(UserRoleEntity entity)
    {
        try
        {
            return new UserRoleDto
            {
                RoleName = entity.RoleName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
