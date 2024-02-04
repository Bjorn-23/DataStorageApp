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
                Id = dto.Id,
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
                Id = dto.UserRole.Id,
                RoleName = dto.UserRole.RoleName,
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
                Id = entity.UserRole.Id,
                RoleName = entity.UserRole.RoleName,
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
                Id = entity.Id,
                RoleName = entity.RoleName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<UserRoleDto> Create(IEnumerable<UserRoleEntity> entities)
    {
        try
        {
            List<UserRoleDto> dtos = new();

            foreach (var entity in entities)
            {
                dtos.Add(Create(entity));
            }

            return dtos;
           
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
