using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class AddressRepository_Tests : BaseRepository<AddressEntity, DataContext>, IAddressRepository
{

    private readonly DataContext _context;

    public AddressRepository_Tests() : base(new DataContext(new DbContextOptionsBuilder<DataContext>()
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
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };

        //Act
        var createResult = addressRepository.Create(address);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Storgatan 1", createResult.StreetName);
        Assert.Equal("222 22", createResult.PostalCode);
    }

    [Fact]
    public void CreateShould_NotCreateOneUserInDatabase_ReturnNulll()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { };

        //Act
        var createResult = addressRepository.Create(address);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllUsersFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        //Act
        var getResult = addressRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Storgatan 1", getResult.FirstOrDefault()!.StreetName);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoUsersInDatabase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = addressRepository.Create(address);

        //Act
        var getResult = addressRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyUserWithTheSuppliedRoleExists_ReturnThatUserFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        //Act
        var getResult = addressRepository.GetAllWithPredicate(x => x.StreetName == createResult.StreetName && x.PostalCode == createResult.PostalCode);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("222 22", getResult.FirstOrDefault()!.PostalCode);
        Assert.Equal("Storgatan 1", getResult.FirstOrDefault()!.StreetName);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoUserWithTheSuppliedRoleExists_ReturnEmptyList()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);
        AddressEntity otherAddress = new() { StreetName = "Andragatan 2", PostalCode = "111 11" };

        //Act
        var getResult = addressRepository.GetAllWithPredicate(x => x.StreetName == otherAddress.StreetName && x.PostalCode == otherAddress.PostalCode);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfUserExists_ReturnOneUserFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        //Act
        var getResult = addressRepository.GetOne(x => x.StreetName == createResult.StreetName && x.PostalCode == createResult.PostalCode);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Storgatan 1", getResult.StreetName);
        Assert.Equal("222 22", getResult.PostalCode);
    }


    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = addressRepository.Create(address);

        //Act
        var getResult = addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);

        //Assert
        Assert.Null(getResult);
    }

    [Fact] // CANT CHANGE SINCE RoleName is primary Key
    public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = createResult.Id, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country };

        //Act
        var updatedResult = addressRepository.Update(createResult, updatedAddress);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Andragatan 2", updatedResult.StreetName);
        Assert.NotEqual("222 22", updatedResult.PostalCode);
    }

    [Fact]
    public void UpdateShould_FailToUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = 0, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country }; //changes Id which should make it fail.

        //Act
        var updatedResult = addressRepository.Update(createResult, updatedAddress);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]  // CANT CHANGE SINCE RoleName is primary Key
    public void UpdateWithPredicate_Should_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = createResult.Id, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country };

        //Act
        var updatedResult = addressRepository.Update(x => x.Id == createResult.Id, updatedAddress);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Andragatan 2", updatedResult.StreetName);
        Assert.NotEqual("Storgatan 1", updatedResult.StreetName);
    }

    [Fact]
    public void UpdateWithPredicate_Should_NotUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = 0, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country }; //changes Id which should make it fail.


        //Act
        var updatedResult = addressRepository.Update(x => x.Id == createResult.Id, updatedAddress);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        //Act
        var updatedResult = addressRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = addressRepository.Create(address);

        //Act
        var updatedResult = addressRepository.Delete(x => x.Id == address.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);

        //Act
        var updatedResult = addressRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = addressRepository.Create(address);

        //Act
        var updatedResult = addressRepository.Delete(address);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = addressRepository.Create(address);


        //Act
        var updatedResult = addressRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        //Arrange
        var addressRepository = new AddressRepository_Tests();
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = addressRepository.Create(address);


        //Act
        var updatedResult = addressRepository.Exists(x => x.Id == address.Id);

        //Assert
        Assert.False(updatedResult);
    }
}