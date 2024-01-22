using Business.Dtos;
using Business.Factories;
using Business.Utils;
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

    public UserDto CreateUser(UserDto user, string password)
    {
        try
        {
            var userRoleEntity = UserRoleFactory.Create(user);
            var userRole = _roleRepository.GetOne(x => x.RoleName == userRoleEntity.RoleName);

            if (userRole == null)
                userRole = _roleRepository.Create(userRoleEntity);           

            var userExists = _userRepository.GetOne(x => x.Email == user.Email);
            if (userExists == null)
            {
                UserEntity userEntity = new()
                {
                    Email = user.Email,
                    Password = PasswordGenerator.GenerateSecurePassword(password),
                    UserRoleName = userRole.RoleName
                };

                var newUserEntity = _userRepository.Create(userEntity);
                if (newUserEntity != null)
                {
                    return user = UserFactory.Create(newUserEntity);
                }
            }
            else
                return user = UserFactory.Create(userExists);

        } catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserEntity GetOne(CustomerEntity entity)
    {
        try
        {
            var result = _userRepository.GetOne(x => x.Email == entity.EmailId);
            if (result != null)
            {
                return result;
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