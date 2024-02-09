using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests.Service_Tests;

public class CustomerService_Tests
{
    private readonly UserRoleRepository _userRoleRepository;
    private readonly UserRepository _userRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly Customer_AddressRepository _customer_AddressRepository;
    private readonly AddressRepository _addressRepository;
    private readonly CustomerService _customerService;

    public CustomerService_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _userRoleRepository = new UserRoleRepository(context);
        _userRepository = new UserRepository(context);
        _customerRepository = new CustomerRepository(context);
        _addressRepository = new AddressRepository(context);
        _customer_AddressRepository = new Customer_AddressRepository(context);
        _customerService = new CustomerService(_customerRepository, _userRepository);
    }

    [Fact]
    public void CreateCustomer_ShouldCreateANewCustomerInDatabaseAnd_ReturnCustomer()
    {
        //Arrange
        var role = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userRepository.Create(new UserEntity() { Id ="1", Email = "bjorn@email.com", Password = "Bytmig123!", SecurityKey="2", IsActive = true, UserRoleId = 1 });
        var customer = new CustomerDto() { FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" };

        //Act
        var createResult = _customerService.CreateCustomer(customer);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("bjorn@email.com", createResult.EmailId);
    }

    [Fact]
    public void GetOneCustomerWithDetails_ShouldGetExistingCustomerFromDatabaseAnd_ReturnCustomerWithRoleAndAddresses()
    {
        //Arrange
        var customerAddress = _customer_AddressRepository.Create(new Customer_AddressEntity() { AddressId = 1, CustomerId = "1" });
        var address = _addressRepository.Create(new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var role = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userRepository.Create(new UserEntity() { Id = "1", Email = "bjorn@email.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var customer = _customerService.CreateCustomer(new CustomerDto() { FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });

        //Act
        var getResult = _customerService.GetOneCustomerWithDetails(customer);

        //Assert
        Assert.NotNull(getResult.customer);
        Assert.NotNull(getResult.userRole);
        Assert.NotEmpty(getResult.address);
        Assert.Equal("Storgatan 1", getResult.address.FirstOrDefault()!.StreetName);
        Assert.Equal("bjorn@email.com", getResult.customer.EmailId);
    }

    [Fact]
    public void GetOneCustomer_ShouldGetExistingCustomerFromDatabaseAnd_ReturnCustomer()
    {
        //Arrange
        var role = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userRepository.Create(new UserEntity() { Id = "1", Email = "bjorn@email.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var customer = _customerService.CreateCustomer(new CustomerDto() { FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });

        //Act
        var getResult = _customerService.GetOneCustomer(customer);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@email.com", getResult.EmailId);
    }

    [Fact]
    public void GetAll_ShouldGetAllExistingCustomersFromDatabaseAnd_ReturnListOfCustomers()
    {
        //Arrange
        var role = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userRepository.Create(new UserEntity() { Id = "1", Email = "bjorn@email.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var customer = _customerService.CreateCustomer(new CustomerDto() { FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });

        //Act
        var getAllResult = _customerService.GetAll();

        //Assert
        Assert.NotNull(getAllResult);
        Assert.Equal("bjorn@email.com", getAllResult.FirstOrDefault()!.EmailId);
    }

    [Fact]
    public void UpdateCustomer_ShouldUpdateExistingCustomerInDatabaseAnd_ReturnUpdatedCustomer()
    {
        //Arrange
        var role = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userRepository.Create(new UserEntity() { Id = "1", Email = "bjorn@email.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var customer = _customerService.CreateCustomer(new CustomerDto() { FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });

        var updatedCustomerResults = new CustomerDto() { Id = customer.Id, FirstName = "Arne", LastName = "Svensson", EmailId = "arne@email.com", PhoneNumber = "0987654321" };
        //Act
        var updateResult = _customerService.UpdateCustomer(customer, updatedCustomerResults);

        //Assert
        Assert.NotNull(updateResult);
        Assert.Equal("arne@email.com", updateResult.EmailId);
    }

    [Fact]
    public void DeleteCustomer_ShouldDeleteExistingCustomerInDatabaseAnd_ReturnDeletedCustomer()
    {
        //Arrange
        var role = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userRepository.Create(new UserEntity() { Id = "1", Email = "bjorn@email.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var customer = _customerService.CreateCustomer(new CustomerDto() { FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });
        string option = "1";
        
        //Act
        var deleteResult = _customerService.DeleteCustomer(customer, option);

        //Assert
        Assert.NotNull(deleteResult);
        Assert.Equal("bjorn@email.com", deleteResult.EmailId);
    }

}
