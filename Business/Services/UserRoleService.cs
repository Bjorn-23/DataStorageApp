using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class UserRoleService(IUserRoleRepository userRoleRepository)
{
    private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;

    public UserEntity GetOrCreateRole(UserDto user)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(user.UserRoleName)) // changed from is nullOrEmpty
            {
                var userRoleEntity = UserRoleFactory.Create(user);
                UserRoleEntity userRole = _userRoleRepository.GetOne(x => x.RoleName == userRoleEntity.RoleName);
                UserEntity roleEntity = new();

                if (userRole != null)
                {
                    roleEntity.UserRoleName = userRole.RoleName;
                    return roleEntity;
                }
                else
                {
                    var newUserRole = _userRoleRepository.Create(userRoleEntity);
                    if (newUserRole != null)
                    {
                        roleEntity.UserRoleName = newUserRole.RoleName;
                        return roleEntity;
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserRoleDto UpdateRole(UserRoleDto user) // very basic, might want to include checks.
    {
        try
        {
            var entity = UserRoleFactory.Create(user);
            var result = _userRoleRepository.Update(x => x.RoleName == user.RoleName, entity);
            var dto = UserRoleFactory.Create(result);
            return dto;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserRoleDto DeleteRole(UserRoleDto role) // very basic, might want to include checks.
    {
        try
        {
            var existingRole = _userRoleRepository.GetOne(x => x.RoleName == role.RoleName);
            var result = _userRoleRepository.Delete(existingRole);
            if (result)
            {
                return UserRoleFactory.Create(existingRole);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}