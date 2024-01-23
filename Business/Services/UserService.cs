using Business.Dtos;
using Business.Factories;
using Business.Utils;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace Business.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserRoleService _userRoleService;

    public UserService(IUserRepository userRepository, UserRoleService userRoleService)
    {
        _userRepository = userRepository;
        _userRoleService = userRoleService;
    }

    public UserDto CreateUser(UserDto user)
    {
        try
        {
            var userRole = _userRoleService.GetOrCreateRole(user);

            var userExists = GetOne(user);
            if (userExists == null)
            {                                
                UserEntity userEntity = new()
                {
                    Email = user.Email,
                    Password = user.Password,
                    SecurityKey = user.SecurityKey,
                    UserRoleName = userRole.UserRoleName
                };

                var newUserEntity = _userRepository.Create(userEntity);
                if (newUserEntity != null)
                    return user = UserFactory.Create(newUserEntity);
            }
            else
                return user = UserFactory.Create(userExists);

        } catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

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
                    LogoutUsers();

                    UserEntity activeUser = new()
                    {
                        Id = existingUser.Id,
                        Email = existingUser.Email,
                        Password = existingUser.Password,
                        SecurityKey = existingUser.SecurityKey,
                        Created = existingUser.Created,
                        isActive = true,
                        UserRoleName = existingUser.UserRoleName    
                    };

                    var setIsActive = _userRepository.Update(existingUser, activeUser);
                    if (setIsActive != null)
                        return true;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public bool UserLogout(UserDto user)
    {
        try
        {
            var existingUser = GetOne(user);
            if (existingUser != null)
            {
                UserEntity inActiveUser = new()
                {
                    Id = existingUser.Id,
                    Email = existingUser.Email,
                    Password = existingUser.Password,
                    SecurityKey = existingUser.SecurityKey,
                    Created = existingUser.Created,
                    isActive = false,
                    UserRoleName = existingUser.UserRoleName
                };

                var setIsActive = _userRepository.Update(existingUser, inActiveUser);
                if (setIsActive != null)
                    return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public UserDto LogoutUsers()
    {
        try
        {
            var userEntities = _userRepository.GetAll();
            var UserDtos = UserFactory.Create(userEntities);

            foreach (var user in UserDtos)
            {
                if (user.isActive)
                {
                    var result = UserLogout(user);
                    if (result)
                        return user;               
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserDto UpdateUser(UserDto user, UserDto newUserDetails)
    {
        try
        {
            var existingUser = GetOne(user);

            if (existingUser.isActive)
            {

                UserEntity updatedUserDetails = new()
                {
                Id = existingUser.Id,
                Email = string.IsNullOrWhiteSpace(newUserDetails.Email) ? existingUser.Email : newUserDetails.Email,
                Password = existingUser.Password,
                SecurityKey = existingUser.SecurityKey,
                Created = existingUser.Created,
                isActive = existingUser.isActive,
                UserRoleName = string.IsNullOrWhiteSpace(newUserDetails.UserRoleName) ? existingUser.UserRoleName : _userRoleService.GetOrCreateRole(newUserDetails).UserRoleName
                };

                if (!string.IsNullOrWhiteSpace(newUserDetails.Password)) // If string has a value in it.
                {
                    var newPasswordAndKey = PasswordGenerator.GenerateSecurePasswordAndKey(updatedUserDetails.Password);
                    updatedUserDetails.Password = newPasswordAndKey.Password;
                    updatedUserDetails.SecurityKey = newPasswordAndKey.SecurityKey;
                }
                
                var result = _userRepository.Update(existingUser, updatedUserDetails);
                if (result != null)
                    return UserFactory.Create(result);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!; ;
    }

    public UserDto DeleteUser(UserDto user)
    {
        try
        {
            var existingUser = GetOne(user);
            if (existingUser != null)
            {
                var result = _userRepository.Delete(existingUser);
                if (result)
                    return user;
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
                return result;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}