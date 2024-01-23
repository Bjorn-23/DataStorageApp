using Business.Dtos;
using Business.Factories;
using Business.Utils;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
            var userRoleEntity = UserRoleFactory.Create(user);
            UserRoleEntity userRole = _roleRepository.GetOne(x => x.RoleName == userRoleEntity.RoleName);

            if (userRole == null)
                userRole = _roleRepository.Create(userRoleEntity);           

            var userExists = _userRepository.GetOne(x => x.Email == user.Email);
            if (userExists == null)
            {                                
                UserEntity userEntity = new()
                {
                    Email = user.Email,
                    Password = user.Password,
                    SecurityKey = user.SecurityKey,
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

    public bool UpdateUser(UserDto user, CustomerDto newUserDetails, string password)
    {
        try
        {
            var existingUser = _userRepository.GetOne(x => x.Email == user.Email);
            var result = PasswordGenerator.VerifyPassword(password, existingUser.SecurityKey, existingUser.Password);

            if (result)
            {
                UserEntity updatedUserDetails = new()
                {
                    Id = existingUser.Id,
                    Email = newUserDetails.EmailId,
                    Password = existingUser.Password,
                    SecurityKey = existingUser.SecurityKey,
                    UserRoleName = existingUser.UserRoleName
                };
                _userRepository.Update(existingUser, updatedUserDetails);

                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
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

    public UserEntity GetOne(UserDto dto)
    {
        try
        {
            var result = _userRepository.GetOne(x => x.Email == dto.Email);
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public bool UserLogin(UserDto user)
    {
        try
        {
            var existingUser = GetOne(user);
            if (existingUser != null)
            {
                var checkPassword = PasswordGenerator.VerifyPassword(user.Password, existingUser.SecurityKey, existingUser.Password);
                if (checkPassword)
                {
                    UserEntity activeUser = new()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Password = user.Password,
                        SecurityKey = user.SecurityKey,
                        Created = user.Created,
                        isActive = true,
                        UserRoleName = user.UserRoleName    
                    };

                    var setIsActive = _userRepository.Update(existingUser, activeUser);
                    if (setIsActive != null)
                    {
                        return true;
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

}