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
                Created = dto.Created,
                isActive = dto.isActive,
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
                Created = entity.Created,
                isActive= entity.isActive,
                UserRoleName = entity.UserRoleName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<UserDto> Create(IEnumerable<UserEntity> entities)
    {
        try
        {
            List<UserDto> list = new();

            foreach (var entity in entities)
            {
                list.Add(Create(entity));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
