using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class Customer_AddressRepository_Tests : BaseRepository<Customer_AddressEntity, DataContext>, ICustomer_AddressRepository
{

    private readonly DataContext _context;

    public Customer_AddressRepository_Tests() : base(new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options))
    {
        _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    }

    [Fact]
    public void CreateShould_CreateOneUserInDatabase_ReturnThatUserIfSuccesfl()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };

        //Act
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal(1, createResult.AddressId);
        Assert.Equal(customerAddress.CustomerId, createResult.CustomerId);
    }

    [Fact]
    public void CreateShould_NotCreateOneUserInDatabase_ReturnNulll()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { };

        //Act
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllUsersFromDataBase()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.AddressId);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoUsersInDatabase()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyUserWithTheSuppliedRoleExists_ReturnThatUserFromDataBase()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.GetAllWithPredicate(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.AddressId);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoUserWithTheSuppliedRoleExists_ReturnEmptyList()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);
        int otherAddress = 2;

        //Act
        var getResult = Customer_AddressRepository.GetAllWithPredicate(x => x.AddressId == otherAddress && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfUserExists_ReturnOneUserFromDataBase()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.GetOne(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.AddressId);
    }

    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.GetOne(x => x.AddressId == customerAddress.AddressId && x.CustomerId == customerAddress.CustomerId);

        //Assert
        Assert.Null(getResult);
    }

    //[Fact] // CANT UPDATE THIS ENTITY SINCE BOTH PROPS ARE PRIMARY KEYS AND MAKE UP A PRIMARY KEY TOGETHER - will leave commented out.
    //public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    //{
    //    // Arrange
    //    var Customer_AddressRepository = new Customer_AddressRepository_Tests();
    //    var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
    //    var createResult = Customer_AddressRepository.Create(customerAddress);

    //    var updatedCustomerAddress = new Customer_AddressEntity() { AddressId = 2, CustomerId = Guid.NewGuid().ToString() };

    //    //Act
    //    var updatedResult = Customer_AddressRepository.Update(createResult, updatedCustomerAddress);

    //    //Assert
    //    Assert.NotNull(updatedResult);
    //    Assert.Equal(2, updatedResult.AddressId);
    //    Assert.NotEqual(1, updatedResult.AddressId);
    //}

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.Delete(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.True(getResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var getResult = Customer_AddressRepository.Delete(x => x.AddressId == customerAddress.AddressId && x.CustomerId == customerAddress.CustomerId);

        //Assert
        Assert.False(getResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var updatedResult = Customer_AddressRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = Customer_AddressRepository.Create(customerAddress);

        //Act
        var updatedResult = Customer_AddressRepository.Delete(customerAddress);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = Customer_AddressRepository.Create(customerAddress);


        //Act
        var getResult = Customer_AddressRepository.Exists(x => x.AddressId == createResult.AddressId && x.CustomerId == createResult.CustomerId);

        //Assert
        Assert.True(getResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        // Arrange
        var Customer_AddressRepository = new Customer_AddressRepository_Tests();
        var customerAddress = new Customer_AddressEntity() { AddressId = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = Customer_AddressRepository.Create(customerAddress);


        //Act
        var getResult = Customer_AddressRepository.Exists(x => x.AddressId == customerAddress.AddressId && x.CustomerId == customerAddress.CustomerId);

        //Assert
        Assert.False(getResult);
    }
}
