using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
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

    //try to make this the main role
    public UserEntity GetOrCreateRole(UserEntity user)
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

}




