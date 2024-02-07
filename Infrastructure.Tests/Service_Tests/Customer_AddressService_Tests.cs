using Business.Dtos;
using Business.Factories;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class Customer_AddressService_Tests
{

    private readonly Customer_AddressService _customer_AddressService;
    private readonly AddressRepository _addressRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly Customer_AddressRepository _customer_AddressRepository;

    public Customer_AddressService_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _addressRepository = new AddressRepository(context);
        _customerRepository = new CustomerRepository(context);
        _customer_AddressRepository = new Customer_AddressRepository(context);
        _customer_AddressService = new Customer_AddressService(_customer_AddressRepository, _addressRepository, _customerRepository);
    }

    [Fact]
    public void CreateCustomerAddress_ShouldCreateANewCustomerAddressInDatabaseAnd_ReturnTrue()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = "1", FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });
        var address = _addressRepository.Create(new AddressEntity() { Id = 1, StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });

        //Act
        var customerAddressResult = _customer_AddressService.CreateCustomer_Address(CustomerFactory.Create(customer), AddressFactory.Create(address));

        //Assert
        Assert.True(customerAddressResult);
    }

    [Fact]
    public void GetCustomerAddress_ShouldGetExistingCustomerAddressInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = "1", FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });
        var address = _addressRepository.Create(new AddressEntity() { Id = 1, StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var customerDto = CustomerFactory.Create(customer);
        var addressDto = AddressFactory.Create(address);
        var customerAddressResult = _customer_AddressService.CreateCustomer_Address(customerDto, addressDto);

        //Act
        var getResult = _customer_AddressService.GetCustomer_Address(customerDto, addressDto);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(getResult.CustomerId, customerDto.Id);
        Assert.Equal(getResult.AddressId, addressDto.Id);
    }


    [Fact]
    public void GetAllCustomerAddress_ShouldGetAllExistingCustomerAddressesInDatabaseAnd_ReturnThem()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = "1", FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });
        var address = _addressRepository.Create(new AddressEntity() { Id = 1, StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var customerDto = CustomerFactory.Create(customer);
        var addressDto = AddressFactory.Create(address);
        var customerAddressResult = _customer_AddressService.CreateCustomer_Address(customerDto, addressDto);

        //Act
        var getResult = _customer_AddressService.GetAllCustomer_Addresses();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(getResult.FirstOrDefault()!.CustomerId, customerDto.Id);
        Assert.Equal(getResult.FirstOrDefault()!.AddressId, addressDto.Id);
    }

    
    [Fact]
    public void DeleteCustomerAddress_ShouldDeleteExistingCustomerAddressInDatabaseAnd_ReturnDeletedCustomer_Address()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = "1", FirstName = "Björn", LastName = "Andersson", EmailId = "bjorn@email.com", PhoneNumber = "0123456789" });
        var address = _addressRepository.Create(new AddressEntity() { Id = 1, StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var customerDto = CustomerFactory.Create(customer);
        var addressDto = AddressFactory.Create(address);
        var customerAddressResult = _customer_AddressService.CreateCustomer_Address(customerDto, addressDto);
  
        //Act
        var getResult = _customer_AddressService.DeleteCustomer_Address(new Customer_AddressDto() { AddressId = 1, CustomerId = "1" });

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(getResult.CustomerId, customerDto.Id);
        Assert.Equal(getResult.AddressId, addressDto.Id);
    }
}
