using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class AddressRepository_Tests
{   
    private readonly AddressRepository _addressRepository;
    public AddressRepository_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _addressRepository = new AddressRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };

        //Act
        var createResult = _addressRepository.Create(address);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Storgatan 1", createResult.StreetName);
        Assert.Equal("222 22", createResult.PostalCode);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var address = new AddressEntity() { };

        //Act
        var createResult = _addressRepository.Create(address);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        //Act
        var getResult = _addressRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Storgatan 1", getResult.FirstOrDefault()!.StreetName);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = _addressRepository.Create(address);

        //Act
        var getResult = _addressRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        //Act
        var getResult = _addressRepository.GetAllWithPredicate(x => x.StreetName == createResult.StreetName && x.PostalCode == createResult.PostalCode);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("222 22", getResult.FirstOrDefault()!.PostalCode);
        Assert.Equal("Storgatan 1", getResult.FirstOrDefault()!.StreetName);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);
        AddressEntity otherAddress = new() { StreetName = "Andragatan 2", PostalCode = "111 11" };

        //Act
        var getResult = _addressRepository.GetAllWithPredicate(x => x.StreetName == otherAddress.StreetName && x.PostalCode == otherAddress.PostalCode);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        //Act
        var getResult = _addressRepository.GetOne(x => x.StreetName == createResult.StreetName && x.PostalCode == createResult.PostalCode);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Storgatan 1", getResult.StreetName);
        Assert.Equal("222 22", getResult.PostalCode);
    }


    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = _addressRepository.Create(address);

        //Act
        var getResult = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);

        //Assert
        Assert.Null(getResult);
    }

    [Fact] // CANT CHANGE SINCE RoleName is primary Key
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = createResult.Id, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country };

        //Act
        var updatedResult = _addressRepository.Update(createResult, updatedAddress);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Andragatan 2", updatedResult.StreetName);
        Assert.NotEqual("222 22", updatedResult.PostalCode);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = 0, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country }; //changes Id which should make it fail.

        //Act
        var updatedResult = _addressRepository.Update(createResult, updatedAddress);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]  // CANT CHANGE SINCE RoleName is primary Key
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = createResult.Id, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country };

        //Act
        var updatedResult = _addressRepository.Update(x => x.Id == createResult.Id, updatedAddress);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Andragatan 2", updatedResult.StreetName);
        Assert.NotEqual("Storgatan 1", updatedResult.StreetName);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        AddressEntity updatedAddress = new() { Id = 0, StreetName = "Andragatan 2", PostalCode = "111 11", City = address.City, Country = address.Country }; //changes Id which should make it fail.


        //Act
        var updatedResult = _addressRepository.Update(x => x.Id == createResult.Id, updatedAddress);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        //Act
        var updatedResult = _addressRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = _addressRepository.Create(address);

        //Act
        var updatedResult = _addressRepository.Delete(x => x.Id == address.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);

        //Act
        var updatedResult = _addressRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = _addressRepository.Create(address);

        //Act
        var updatedResult = _addressRepository.Delete(address);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        var createResult = _addressRepository.Create(address);


        //Act
        var updatedResult = _addressRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var address = new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" };
        //var createResult = _addressRepository.Create(address);


        //Act
        var updatedResult = _addressRepository.Exists(x => x.Id == address.Id);

        //Assert
        Assert.False(updatedResult);
    }
}
