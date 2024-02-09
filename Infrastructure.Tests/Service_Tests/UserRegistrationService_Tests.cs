using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class UserRegistrationService_Tests
{
    private readonly UserRepository _userRepository;
    private readonly UserRoleRepository _userRoleRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly AddressRepository _addressRepository;
    private readonly Customer_AddressRepository _customerAddressRepository;
    private readonly UserService _userService;
    private readonly UserRoleService _userRoleService;
    private readonly CustomerService _customerService;
    private readonly AddressService _addressService;
    private readonly Customer_AddressService _customerAddressService;
    private readonly UserRegistrationService _userRegistrationService;

    public UserRegistrationService_Tests()
        {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _userRepository = new UserRepository(context);
        _userRoleRepository = new UserRoleRepository(context);
        _customerRepository = new CustomerRepository(context);
        _addressRepository = new AddressRepository(context);
        _customerAddressRepository = new Customer_AddressRepository(context);
        _userService = new UserService(_customerRepository, _userRepository);
        _userRoleService = new UserRoleService(_userRoleRepository);
        _customerService = new CustomerService(_customerRepository, _userRepository);
        _addressService = new AddressService(_addressRepository, _customerAddressRepository, _userService);
        _customerAddressService = new Customer_AddressService(_customerAddressRepository, _addressRepository, _customerRepository);
        _userRegistrationService = new UserRegistrationService(_userService, _customerService, _addressService, _customerAddressService, _userRoleService);
    }

    [Fact]
    public void CreateNewUser_ShouldCreateANewUser_Customer_AddressAndCustomerAddressAnd_ReturnCustomerAndAddress()
    {
        //Arrange        
        var user = new UserRegistrationDto()
        {
            FirstName = "Björn",
            LastName = "Andersson",
            PhoneNumber = "1234567890",
            UserRoleName = "Admin",
            StreetName = "Storgatan 1",
            PostalCode = "123 45",
            City = "Storstan",
            Country = "Sverige",
            Email = "bjorn@domain.com",
            Password = "Bytmig123!",
        };

        //Act
        var createdUser = _userRegistrationService.CreateNewUser(user);

        //Assert
        Assert.NotNull(createdUser.customer);
        Assert.NotNull(createdUser.address);
        Assert.Equal("Björn", createdUser.customer.FirstName);
        Assert.Equal("Storgatan 1", createdUser.address.StreetName);
        Assert.Equal(user.Email, createdUser.customer.EmailId);
    }
}
