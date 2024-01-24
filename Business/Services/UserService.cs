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
    private readonly ICustomerRepository _customerRepository;
    private readonly UserRoleService _userRoleService;

    public UserService(IUserRepository userRepository, UserRoleService userRoleService, ICustomerRepository customerRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
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
                    LogoutUsers(); // Remove comment If only one user should be active at any time on a single machine.

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
                UserEntity inactiveUser = new()
                {
                    Id = existingUser.Id,
                    Email = existingUser.Email,
                    Password = existingUser.Password,
                    SecurityKey = existingUser.SecurityKey,
                    Created = existingUser.Created,
                    isActive = false,
                    UserRoleName = existingUser.UserRoleName
                };

                var setIsActive = _userRepository.Update(existingUser, inactiveUser);
                if (setIsActive != null)
                    return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public UserDto LogoutUsers() //Inefficient in large databases, but works for now as a logout button that doesnt require user input.
    {
        try
        {
            var userEntities = _userRepository.GetAllWithPredicate(x => x.isActive == true);
            var UserDtos = UserFactory.Create(userEntities);

            foreach (var user in UserDtos)
            {
                var result = UserLogout(user);
                return user;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserEntity FindRoleOfActiveUser() //Inefficient in large databases, but works for now as a logout button that doesnt require user input.
    {
        try
        {
            var userEntities = _userRepository.GetAllWithPredicate(x => x.isActive == true);

            foreach (var user in userEntities)
            {
                return user;
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
            var checkRole = FindRoleOfActiveUser();

            if (existingUser.isActive || checkRole.UserRoleName.ToString() == "Admin") // add || statement to if user role == "Admin" so an admin can change when logged in.
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

                // Generates new .SecurityKey and .Password combination if a new password was submitted
                if (!string.IsNullOrWhiteSpace(newUserDetails.Password))
                {
                    var newPasswordAndKey = PasswordGenerator.GenerateSecurePasswordAndKey(updatedUserDetails.Password);
                    updatedUserDetails.Password = newPasswordAndKey.Password;
                    updatedUserDetails.SecurityKey = newPasswordAndKey.SecurityKey;                                        
                }

                //Changes .EmailId in Customers table if a new email was submitted
                if (!string.IsNullOrWhiteSpace(newUserDetails.Email))
                {
                    var result = _userRepository.Update(existingUser, updatedUserDetails);
                    if (result != null)
                    {
                        var existingCustomer = _customerRepository.GetOne(x => x.Id == result.Id);

                        CustomerEntity newEmail = new()
                        {
                            Id = existingCustomer.Id,
                            FirstName = existingCustomer.FirstName,
                            LastName = existingCustomer.LastName,
                            EmailId = result.Email,
                            PhoneNumber = existingCustomer.PhoneNumber,
                        };
                        var updateCustomerEmail = _customerRepository.Update(x => x.Id == result.Id, newEmail);
                        if (updateCustomerEmail != null)
                            return UserFactory.Create(result);
                    }
                }
                else
                {
                    var result = _userRepository.Update(existingUser, updatedUserDetails);
                    if (result != null)
                        return UserFactory.Create(result);
                }
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
            var checkRole = FindRoleOfActiveUser();

            // Add extra check to see if user should be deleted?

            if (existingUser.isActive || checkRole.UserRole.ToString() == "Admin")
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
    
    public UserEntity GetOne(CustomerEntity entity) // Currently only used in customer service.
    {
        try
        {
            var result = _userRepository.GetOne(x => x.Email == entity.EmailId);
            if (result != null)
                return result;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}