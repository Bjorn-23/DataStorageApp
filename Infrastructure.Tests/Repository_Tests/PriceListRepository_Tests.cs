using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repository_Tests;

public class PriceListRepository_Tests
{
    private readonly PriceListRepository _priceListRepository;

    public PriceListRepository_Tests()
    {
        var context = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _priceListRepository = new PriceListRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };

        //Act
        var createResult = _priceListRepository.Create(priceList);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("SEK", createResult.UnitType);
        Assert.Equal(2, createResult.Price);
        Assert.Equal(1, createResult.Id);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var priceList = new PriceListEntity() { };

        //Act
        var createResult = _priceListRepository.Create(priceList);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        //Act
        var getResult = _priceListRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(2, getResult.FirstOrDefault()!.Price);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = priceListRepository.Create(priceList);

        //Act
        var getResult = _priceListRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        //Act
        var getResult = _priceListRepository.GetAllWithPredicate(x => x.Id == createResult.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.DiscountPrice);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        var otherpriceList = new PriceListEntity() { Price = 1, DiscountPrice = 0, UnitType = "EUR" };

        //Act
        var getResult = _priceListRepository.GetAllWithPredicate(x => x.UnitType == otherpriceList.UnitType);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        //Act
        var getResult = _priceListRepository.GetOne(x => x.Id == createResult.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.Id);
        Assert.Equal(1, getResult.DiscountPrice);
    }


    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = _priceListRepository.Create(priceList);

        //Act
        var getResult = _priceListRepository.GetOne(x => x.Price == priceList.Price);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = createResult.Id, Price = 1, DiscountPrice = 0, UnitType = "EUR" };

        //Act
        var updatedResult = _priceListRepository.Update(createResult, updatedPriceList);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("EUR", updatedResult.UnitType);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = 0, Price = 1, DiscountPrice = 0, UnitType = "EUR" }; // Id changed, should fail

        //Act
        var updatedResult = _priceListRepository.Update(createResult, updatedPriceList);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = createResult.Id, Price = 1, DiscountPrice = 0, UnitType = "EUR" };

        //Act
        var updatedResult = _priceListRepository.Update(x => x.Id == createResult.Id, updatedPriceList);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("EUR", updatedResult.UnitType);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        var updatedPriceList = new PriceListEntity() { Id = 0, Price = 1, DiscountPrice = 0, UnitType = "EUR" }; // Id changed, should fail

        //Act
        var updatedResult = _priceListRepository.Update(x => x.Id == createResult.Id, updatedPriceList);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        //Act
        var updatedResult = _priceListRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = _priceListRepository.Create(priceList);

        //Act
        var updatedResult = _priceListRepository.Delete(x => x.Id == priceList.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        //Act
        var updatedResult = _priceListRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = _priceListRepository.Create(priceList);

        //Act
        var updatedResult = _priceListRepository.Delete(priceList);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        var createResult = _priceListRepository.Create(priceList);

        //Act
        var updatedResult = _priceListRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var priceList = new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" };
        //var createResult = _priceListRepository.Create(priceList);


        //Act
        var updatedResult = _priceListRepository.Exists(x => x.Id == priceList.Id);

        //Assert
        Assert.False(updatedResult);
    }
}