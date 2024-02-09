using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class UserRoleService(IUserRoleRepository userRoleRepository)
{
    private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;

    /// <summary>
    /// Checks if input roleName exists, if tru then returns it, otherwise creates new UserRole with the correct UserRole.Id for the user to be associated with.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns>UserEntity</returns>
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

    /// <summary>
    /// Gets all existing UserRoles from database.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Updates a userRole with new details. Doesnt currently have checks but should probably have a password check to make sure not anyone can make themselves an admin.
    /// </summary>
    /// <param name="userRole"></param>
    /// <param name="updatedUserRoleDetails"></param>
    /// <returns></returns>
    public UserRoleDto UpdateRole(UserRoleDto userRole, UserRoleDto updatedUserRoleDetails) // very basic, might want to include checks.
    {
        try
        {
            var updatedEntityDetails = UserRoleFactory.Create(updatedUserRoleDetails);
            var result = _userRoleRepository.Update(x => x.RoleName == userRole.RoleName, updatedEntityDetails);
            var dto = UserRoleFactory.Create(result);
            return dto;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    /// <summary>
    /// Deletes a userRole. Doesnt currently have checks but should probably have a password check to make sure not anyone can delete a role.
    /// </summary>
    /// <param name="userRole"></param>
    /// <param name="updatedUserRoleDetails"></param>
    /// <returns></returns>
    public UserRoleDto DeleteRole(UserRoleDto role)
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