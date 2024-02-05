using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Service_Tests;

public class UserService_Test
{
    private readonly UserRepository _userRepository;
    private readonly UserRoleRepository _userRoleRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly AddressRepository _addressRepository;
    private readonly Customer_AddressRepository _customerAddressRepository;
    private readonly UserService _userService;

    public UserService_Test()
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
        
    }

    [Fact]
    public void Create()
    {
        //Arrange        
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = 1, Password = "Bytmig123!", SecurityKey = "bla" };
        
        //Act
        var createdUser = _userService.CreateUser(user);

        //Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Email, createdUser.Email);
    }

    [Fact]
    public void Get()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = "bla" };
        var createdUser = _userService.CreateUser(user);

        //Act
        var getResult = _userService.GetOne(createdUser);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(createdUser.Email, getResult.Email);
        Assert.Equal(createdUser.Id, getResult.Id);
    }


    [Fact]
    public void Update()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = "bla" };
        var createdUser = _userService.CreateUser(user);
        var customer = new CustomerEntity() { EmailId = createdUser.Email, FirstName = "Björn", LastName = "Andersson", Id = createdUser.Id, PhoneNumber = "0789456123" };
        var address = _addressRepository.Create(new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" });
        var customer_address = _customerAddressRepository.Create(new Customer_AddressEntity() { AddressId = address.Id, CustomerId = customer.Id });
        var createdCustomer = _customerRepository.Create(customer);

        var updatedUserDetails = new UserDto() { Id = createdUser.Id, Email = "bjorn@mail.com", Created = createdUser.Created, IsActive = createdUser.IsActive, UserRoleId = createdUser.UserRoleId, Password = "Bytmig123!", SecurityKey = "nja" };

        //Act
        var updatedResults = _userService.UpdateUser(createdUser, updatedUserDetails);


        //Assert
        Assert.NotNull(updatedResults);
        Assert.Equal(createdUser.Id, updatedResults.Id);
        Assert.NotEqual(createdUser.Email, updatedResults.Email);
    }

    [Fact]
    public void Delete()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = "bla" };
        var createdUser = _userService.CreateUser(user);

        //Act
        var deletedResults = _userService.DeleteUser(createdUser);


        //Assert
        Assert.NotNull(deletedResults);
        Assert.Equal(createdUser.Id, deletedResults.Id);       
    }

    [Fact]
    public void UserLogin()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = false, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = null! };
        var createdUser = _userService.CreateUser(user);

        //Act
        var LoggedInResults = _userService.UserLogin(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!" });
        var getUser = _userService.GetOne(createdUser);

        //Assert
        Assert.True(LoggedInResults);
        Assert.True(getUser.IsActive);
    }
    [Fact]
    public void UserLogOut()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = null! };
        var createdUser = _userService.CreateUser(user);

        //Act
        var LoggedOutResults = _userService.UserLogOut(createdUser);
        var getUser = _userRepository.GetOne(x => x.Email == "bjorn@domain.com");

        //Assert
        Assert.True(LoggedOutResults);
        Assert.False(getUser.IsActive);
    }

    [Fact]
    public void LogOutActiveUsers()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = null! };
        var createdUser = _userService.CreateUser(user);

        //Act
        bool LoggedOutResults = _userService.LogOutActiveUser();
        var getUser = _userRepository.GetOne(x => x.Email == "bjorn@domain.com");

        //Assert
        Assert.True(LoggedOutResults);
        Assert.False(getUser.IsActive);

    }

    [Fact]
    public void isUserActive()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = new UserDto() { Email = "bjorn@domain.com", Created = DateTime.Now, IsActive = true, UserRoleId = userRole.Id, Password = "Bytmig123!", SecurityKey = null! };
        var createdUser = _userService.CreateUser(user);

        //Act
        var findRoleResults = _userService.isUserActive();
       
        //Assert
        Assert.NotNull(findRoleResults);
        Assert.True(findRoleResults.IsActive);
    }
}
