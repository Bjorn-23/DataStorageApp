using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IUserRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public UserDto CreateUser(UserDto user)
    {
        try
        {
            UserRoleEntity userRoleEntity = new()
            {
                RoleName = user.UserRoleName
            };
                
            var roleExists = _roleRepository.Exists(x => x.RoleName == user.UserRoleName);

            if (roleExists)
            {
                userRoleEntity = _roleRepository.GetOne(x => x.RoleName == user.UserRoleName);
            }
            else
            {
                userRoleEntity = _roleRepository.Create(userRoleEntity);
            }

            var userExists = _userRepository.Exists(x => x.Email == user.Email);
            if (!userExists)
            {
                UserEntity userEntity = new()
                {
                    Email = user.Email,
                    Password = user.Password,
                    UserRoleName = userRoleEntity.RoleName  
                };

                var newUserEntity = _userRepository.Create(userEntity);
                if (newUserEntity != null)
                {
                    return user;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserDto GetOneUserRole(CustomerEntity entity)
    {
        try
        {
            var result = _userRepository.GetOne(x => x.Email == entity.EmailId);
            if (result != null)
            {
                UserDto user = new()
                {
                    UserRoleName = result.UserRoleName,
                };

                return user;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;

    }
}


//try
//{

//}
//catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }