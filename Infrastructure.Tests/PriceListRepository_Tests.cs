using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class PriceListRepository_Tests : BaseRepository<PriceListEntity, ProductCatalog>, IPriceListRepository
{

    private readonly ProductCatalog _context;

    public PriceListRepository_Tests() : base(new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options))
    {
        _context = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    }

    [Fact]
    public void CreateShould_CreateOneCategoryInDatabase_AndReturnCategory()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };

        //Act
        var createResult = priceListRepository.Create(priceList);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("SEK", createResult.UnitType);
        Assert.Equal(2, createResult.Price);
        Assert.Equal(1, createResult.Id);
    }


    [Fact]
    public void CreateShould_NotCreateOneUserInDatabase_ReturnNulll()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() {};

        //Act
        var createResult = priceListRepository.Create(priceList);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllUsersFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        //Act
        var getResult = priceListRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(2, getResult.FirstOrDefault()!.Price);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoUsersInDatabase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = priceListRepository.Create(priceList);

        //Act
        var getResult = priceListRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyUserWithTheSuppliedRoleExists_ReturnThatUserFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        //Act
        var getResult = priceListRepository.GetAllWithPredicate(x => x.Id == createResult.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.DiscountPrice);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoUserWithTheSuppliedRoleExists_ReturnEmptyList()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        var otherpriceList = new PriceListEntity() { Price = 1, DiscountPrice = 0, UnitType = "EUR" };

        //Act
        var getResult = priceListRepository.GetAllWithPredicate(x => x.UnitType == otherpriceList.UnitType);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfUserExists_ReturnOneUserFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        //Act
        var getResult = priceListRepository.GetOne(x => x.Price == createResult.Price && x.DiscountPrice == createResult.DiscountPrice && x.UnitType == createResult.UnitType);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.Id);
        Assert.Equal(1, getResult.DiscountPrice);
    }


    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = priceListRepository.Create(priceList);

        //Act
        var getResult = priceListRepository.GetOne(x => x.Price == priceList.Price);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = createResult.Id,  Price = 1, DiscountPrice = 0, UnitType = "EUR" };

        //Act
        var updatedResult = priceListRepository.Update(createResult, updatedPriceList);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("EUR", updatedResult.UnitType);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateShould_FailToUpdateExistingUser_ReturnNull()
    {
        //Arrange
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = 0, Price = 1, DiscountPrice = 0, UnitType = "EUR" }; // Id changed, should fail

        //Act
        var updatedResult = priceListRepository.Update(createResult, updatedPriceList);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_Should_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = createResult.Id, Price = 1, DiscountPrice = 0, UnitType = "EUR" };

        //Act
        var updatedResult = priceListRepository.Update(x => x.Id == createResult.Id, updatedPriceList);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("EUR", updatedResult.UnitType);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateWithPredicate_Should_NotUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = 0, Price = 1, DiscountPrice = 0, UnitType = "EUR" }; // Id changed, should fail

        //Act
        var updatedResult = priceListRepository.Update(x => x.Id == createResult.Id, updatedPriceList);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        //Act
        var updatedResult = priceListRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = priceListRepository.Create(priceList);

        //Act
        var updatedResult = priceListRepository.Delete(x => x.Id == priceList.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        //Act
        var updatedResult = priceListRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = priceListRepository.Create(priceList);

        //Act
        var updatedResult = priceListRepository.Delete(priceList);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = priceListRepository.Create(priceList);

        //Act
        var updatedResult = priceListRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        //Arrange
        var priceListRepository = new PriceListRepository_Tests();
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = priceListRepository.Create(priceList);


        //Act
        var updatedResult = priceListRepository.Exists(x => x.Id == priceList.Id);

        //Assert
        Assert.False(updatedResult);
    }
}