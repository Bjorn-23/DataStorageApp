using Business.Dtos;
using Business.Factories;
using Business.Utils;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class UserService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly UserRoleService _userRoleService;

    public UserService(ICustomerRepository customerRepository, IUserRepository userRepository, UserRoleService userRoleService)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _userRoleService = userRoleService;
    }

    public UserDto CreateUser(UserDto user)
    {
        try
        {
            var userExists = _userRepository.GetOne(x => x.Email == user.Email);
            if (userExists == null)
            {
                var userRole = _userRoleService.GetOrCreateRole(user);

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
    
    public UserDto GetOne(UserDto userDto) // Currently not used.
    {
        try
        {
            var activeUser = FindRoleOfActiveUser();
            if ( activeUser.Email == userDto.Email || activeUser.UserRoleName == "Admin")
            {
                var result = _userRepository.GetOne(x => x.Email == userDto.Email);
                if (result != null)
                    return UserFactory.Create(result);                
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public UserDto UpdateUser(UserDto user, UserDto newUserDetails)
    {
        try
        {
            var existingUser = _userRepository.GetOne(x => x.Email == user.Email);
            var checkRole = FindRoleOfActiveUser();

            if (existingUser.IsActive || checkRole.UserRoleName == "Admin") // add || statement to if user role == "Admin" so an admin can change when logged in.
            {

                UserEntity updatedUserDetails = new()
                {
                    Id = existingUser.Id,
                    Email = string.IsNullOrWhiteSpace(newUserDetails.Email) ? existingUser.Email : newUserDetails.Email,
                    Password = existingUser.Password,
                    SecurityKey = existingUser.SecurityKey,
                    Created = existingUser.Created,
                    IsActive = existingUser.IsActive,
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
            var existingUser = _userRepository.GetOne(x => x.Email == user.Email);
            var checkRole = FindRoleOfActiveUser();

            // Add extra check to see if user should be deleted?

            if (existingUser.IsActive || checkRole.UserRole.ToString() == "Admin")
            {
                var result = _userRepository.Delete(existingUser);
                if (result)
                    return user;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return null!;


    }
    
    public bool UserLogin(UserDto user)
    {
        try
        {
            var existingUser = _userRepository.GetOne(x => x.Email == user.Email);
            if (existingUser != null)
            {
                var checkPassword = PasswordGenerator.VerifyPassword(user.Password, existingUser.SecurityKey, existingUser.Password);
                if (checkPassword)
                {
                    LogOutActiveUser(); // Remove comment If only one user should be active at any time on a single machine.

                    UserEntity activeUser = new()
                    {
                        Id = existingUser.Id,
                        Email = existingUser.Email,
                        Password = existingUser.Password,
                        SecurityKey = existingUser.SecurityKey,
                        Created = existingUser.Created,
                        IsActive = true,
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

    public bool UserLogOut(UserDto user)
    {
        try
        {
            var existingUser = _userRepository.GetOne(x => x.Email == user.Email);
            if (existingUser != null)
            {
                UserEntity inactiveUser = new()
                {
                    Id = existingUser.Id,
                    Email = existingUser.Email,
                    Password = existingUser.Password,
                    SecurityKey = existingUser.SecurityKey,
                    Created = existingUser.Created,
                    IsActive = false,
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

    public bool LogOutActiveUser()
    {
        try
        {
            var userEntities = _userRepository.GetAllWithPredicate(x => x.IsActive == true);
            var userDtos = UserFactory.Create(userEntities);

            // Use FirstOrDefault if only one user can be logged in at a time
            var result = UserLogOut(userDtos.FirstOrDefault()!);

            // Use foreach loop if app is changed to be able to have more users logged in at once. 
            //foreach (var user in userDtos)
            //{
            //    var result = UserLogOut(user);
            //}

            if (result)
            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public UserEntity FindRoleOfActiveUser()
    {
        try
        {
            var userEntities = _userRepository.GetAllWithPredicate(x => x.IsActive == true);

            foreach (var user in userEntities)
            {
                return user;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}