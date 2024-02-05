using Business.Dtos;
using Business.Utils;
using System.Diagnostics;

namespace Business.Services;

public class UserRegistrationService
{
    private readonly UserService _userService;
    private readonly CustomerService _customerService;
    private readonly AddressService _addressService;
    private readonly Customer_AddressService _customerAddressService;
    private readonly UserRoleService _userRoleService;

    public UserRegistrationService(UserService userService, CustomerService customerService, AddressService addressService, Customer_AddressService customerAddressService, UserRoleService userRoleService)
    {
        _userService = userService;
        _customerService = customerService;
        _addressService = addressService;
        _customerAddressService = customerAddressService;
        _userRoleService = userRoleService;
    }

    public (CustomerDto, AddressDto) CreateNewUser(UserRegistrationDto registration)
    {
        try
        {
            bool noEmptyProps = PropCheck.CheckAllPropertiesAreSet(registration);
            if (noEmptyProps)
            {
                var roleId = _userRoleService.GetOrCreateRole(registration.UserRoleName);

                UserDto userDto = new UserDto()
                {
                    Email = registration.Email,
                    Password = registration.Password,
                    UserRoleId = roleId.UserRoleId
                };

                CustomerDto customerDto = new CustomerDto()
                {
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    EmailId = registration.Email,
                    PhoneNumber = registration.PhoneNumber
                };

                AddressDto addressDto = new AddressDto()
                {
                    StreetName = registration.StreetName,
                    PostalCode = registration.PostalCode,
                    City = registration.City,
                    Country = registration.Country
                };


                var userResult = _userService.CreateUser(userDto);
                if (userResult != null)
                {  
                    var customerResult = _customerService.CreateCustomer(customerDto);
                    if (customerResult != null)
                    {
                        var addressResult = _addressService.CreateAddress(addressDto);
                        if (addressResult != null)
                        {
                            var customer_AddressResult = _customerAddressService.CreateCustomer_Address(customerResult, addressResult);
                            if (customer_AddressResult)
                                return (customerDto, addressDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return (null!, null!);    
    }
    
}
