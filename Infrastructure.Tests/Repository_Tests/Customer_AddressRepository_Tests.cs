using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repository_Tests;

public class Customer_AddressRepository_Tests
{

    private readonly Customer_AddressRepository _customer_AddressRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly AddressRepository _addressRepository;

    public Customer_AddressRepository_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _customer_AddressRepository = new Customer_AddressRepository(context);
        _customerRepository = new CustomerRepository(context);
        _addressRepository = new AddressRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };

        //Act
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal(1, createResult.AddressId);
        Assert.Equal(customerAddress.CustomerId, createResult.CustomerId);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { };

        //Act
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = Guid.NewGuid().ToString(), FirstName = "Björn", LastName = "Andersson", EmailId = "Björn@domain.com", PhoneNumber = "0789456123" });
        var address = _addressRepository.Create(new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var customerAddress = new Customer_AddressEntity() { AddressId = address.Id, CustomerId = customer.Id };
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.AddressId);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.GetAllWithPredicate(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.AddressId);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _customer_AddressRepository.Create(customerAddress);
        int otherAddress = 2;

        //Act
        var getResult = _customer_AddressRepository.GetAllWithPredicate(x => x.AddressId == otherAddress && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var customer = _customerRepository.Create(new CustomerEntity() { Id = Guid.NewGuid().ToString(), FirstName = "Björn", LastName = "Andersson", EmailId = "Björn@domain.com", PhoneNumber = "0789456123" });
        var address = _addressRepository.Create(new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "111 11", City = "Storstan", Country = "Sverige" });
        var customerAddress = new Customer_AddressEntity() { AddressId = address.Id, CustomerId = customer.Id };
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.GetOne(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.AddressId);
    }

    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.GetOne(x => x.AddressId == customerAddress.AddressId && x.CustomerId == customerAddress.CustomerId);

        //Assert
        Assert.Null(getResult);
    }

    //[Fact] // CANT UPDATE THIS ENTITY SINCE BOTH PROPS ARE PRIMARY KEYS AND MAKE UP A PRIMARY KEY TOGETHER - will leave commented out.
    //public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    //{
    //    //Arrange
    //    var _customer_AddressRepository = new Customer_AddressRepository_Tests();
    //    var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
    //    var createResult = _customer_AddressRepository.Create(customerAddress);

    //    var updatedCustomerAddress = new Customer_AddressEntity() { AddressId = 2, CustomerId = Guid.NewGuid().ToString() };

    //    //Act
    //    var updatedResult = _customer_AddressRepository.Update(createResult, updatedCustomerAddress);

    //    //Assert
    //    Assert.NotNull(updatedResult);
    //    Assert.Equal(2, updatedResult.AddressId);
    //    Assert.NotEqual(1, updatedResult.AddressId);
    //}

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.Delete(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.True(getResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = _customer_AddressRepository.Delete(x => x.AddressId == customerAddress.AddressId && x.CustomerId == customerAddress.CustomerId);

        //Assert
        Assert.False(getResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var updatedResult = _customer_AddressRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _customer_AddressRepository.Create(customerAddress);

        //Act
        var updatedResult = _customer_AddressRepository.Delete(customerAddress);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _customer_AddressRepository.Create(customerAddress);


        //Act
        var getResult = _customer_AddressRepository.Exists(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.True(getResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _customer_AddressRepository.Create(customerAddress);


        //Act
        var getResult = _customer_AddressRepository.Exists(x => x.AddressId == customerAddress.AddressId && x.CustomerId == customerAddress.CustomerId);

        //Assert
        Assert.False(getResult);
    }
}
