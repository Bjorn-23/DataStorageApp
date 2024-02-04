using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class UserRoleService(IUserRoleRepository userRoleRepository)
{
    private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;

    public UserEntity GetOrCreateRole(string roleName)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(roleName)) // changed from is nullOrEmpty
            {
                UserRoleEntity userRole = _userRoleRepository.GetOne(x => x.RoleName == roleName);
                UserEntity roleEntity = new();

                if (userRole != null)
                {
                    roleEntity.UserRoleId = userRole.Id;

                    return roleEntity;
                }
                else
                {
                    var newUserRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = roleName });
                    if (newUserRole != null)
                    {
                        roleEntity.UserRoleId = newUserRole.Id;
                        return roleEntity;
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<UserRoleDto> GetAll()
    {
        try
        {
            var existingUserRoles = _userRoleRepository.GetAll();
            if (existingUserRoles != null && existingUserRoles.Count() >= 1)
            {
                return UserRoleFactory.Create(existingUserRoles);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<UserRoleDto>();
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