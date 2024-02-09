using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class AddressService_Tests
{
    private readonly AddressRepository _addressRepository;
    private readonly Customer_AddressRepository _customerAddressRepository;
    private readonly UserRepository _userRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly AddressService _addressService;
    private readonly UserService _userService;
    private readonly UserRoleRepository _userRoleRepository;

    public AddressService_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _addressRepository = new AddressRepository( context );
        _customerRepository = new CustomerRepository( context );
        _userRepository = new UserRepository( context );
        _customerAddressRepository = new Customer_AddressRepository( context );
        _userRoleRepository = new UserRoleRepository( context );
        _userService = new UserService(_customerRepository, _userRepository);
        _addressService = new AddressService(_addressRepository, _customerAddressRepository, _userService);        
    }

    [Fact]
    public void CreateAddress_ShouldCreateANewAddressInDatabaseAnd_ReturnAddress()
    {
        //Arrange
        var address = new AddressDto() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" };

        //Act
        var createResult = _addressService.CreateAddress(address);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal(address.StreetName, createResult.StreetName);
    }

    [Fact]
    public void GetOneAddressWithCustomers_ShouldGetOneAddressInDatabaseAnd_ReturnAddressAndAssociatedCustomers()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = "1", FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });
        var customerAddress = _customerAddressRepository.Create(new Customer_AddressEntity() { AddressId = 1, CustomerId = "1" });
        var address = _addressService.CreateAddress(new AddressDto() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });

        //Act
        var getOneResult = _addressService.GetOneAddressWithCustomers(address);

        //Assert
        Assert.NotNull(getOneResult.address);
        Assert.NotNull(getOneResult.customers);
        Assert.NotEmpty(getOneResult.customers);
        Assert.Equal(address.StreetName, getOneResult.address.StreetName);
        Assert.Equal(customer.FirstName, getOneResult.customers.FirstOrDefault()!.FirstName);
    }

    [Fact]
    public void GetOneAddress_ShouldGetOneAddressFromDatabaseAnd_ReturnIt()
    {
        //Arrange
        var address = _addressService.CreateAddress(new AddressDto() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });

        //Act
        var getResult = _addressService.GetOneAddress(address);

        //Assert
        Assert.NotNull(getResult);
        Assert.NotNull(getResult);
        Assert.Equal(address.StreetName, getResult.StreetName);
    }

    [Fact]
    public void GetAll_ShouldGetAllAddressesFromDatabaseAnd_ReturnThem()
    {
        //Arrange
        var address = _addressService.CreateAddress(new AddressDto() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var address2 = _addressService.CreateAddress(new AddressDto() { StreetName = "Storgatan 2", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });

        //Act
        var getResult = _addressService.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.NotEmpty(getResult);
    }

    [Fact]
    public void UpdateAddress_ShouldUpdateExistingAddressInDatabaseAnd_ReturnUpdatedAddress()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var address = _addressService.CreateAddress(new AddressDto() { Id = 1, StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var address2 = new AddressDto() { Id = address.Id, StreetName = address.StreetName, PostalCode = address.PostalCode, City = "MegaStorstan", Country = "Sverige" };

        //Act
        var getResult = _addressService.UpdateAddress(address, address2);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(address.Id, getResult.Id);
        Assert.NotEqual(address.City, getResult.City);
        Assert.Equal(address2.City, getResult.City);
    }

    [Fact]
    public void DeleteAddress_ShouldDeleteExistingAddressInDatabaseAnd_ReturnDeletedAddress()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var address = _addressService.CreateAddress(new AddressDto() { Id = 1, StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });

        //Act
        var deletedResult = _addressService.DeleteAddress(address);

        //Assert
        Assert.NotNull(deletedResult);
        Assert.Equal(address.Id, deletedResult.Id);
    }
}
