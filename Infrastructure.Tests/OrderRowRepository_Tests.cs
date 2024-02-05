using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests;

public class OrderRowRepository_Tests
{

    private readonly OrderRowRepository _orderRowRepository;

    public OrderRowRepository_Tests()
    {
        var context = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _orderRowRepository = new OrderRowRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };

        //Act
        var createResult = _orderRowRepository.Create(orderRow);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Demo1", createResult.ArticleNumber);
        Assert.Equal(1, createResult.Quantity);
        Assert.Equal(10, createResult.OrderRowPrice);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { };

        //Act
        var createResult = _orderRowRepository.Create(orderRow);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var getResult = _orderRowRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(10, getResult.FirstOrDefault()!.OrderRowPrice);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        //var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var getResult = _orderRowRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyEntities_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var getResult = _orderRowRepository.GetAllWithPredicate(x => x.OrderId == createResult.OrderId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.OrderId);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        var otherorderRow = new OrderRowEntity() { Id = 2, Quantity = 2, OrderRowPrice = 20, ArticleNumber = "Demo1", OrderId = 1 };

        //Act
        var getResult = _orderRowRepository.GetAllWithPredicate(x => x.Id == otherorderRow.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1, Id = 1 }; // Id set explicitly, Identity doesnt apply to non Key values from 1 but 0 in Inmemory database

        var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var getResult = _orderRowRepository.GetOne(x => x.Id == createResult.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.Id);
    }


    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        //var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var getResult = _orderRowRepository.GetOne(x => x.Id == orderRow.Id);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        var updatedOrderRow = new OrderRowEntity() { Id = orderRow.Id, Quantity = 2, OrderRowPrice = 20, ArticleNumber = "Demo1", OrderId = 1 };

        //Act
        var updatedResult = _orderRowRepository.Update(createResult, updatedOrderRow);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal(20, updatedResult.OrderRowPrice);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        var updatedOrderRow = new OrderRowEntity() { Id = createResult.Id, Quantity = 2, OrderRowPrice = 20, ArticleNumber = "Demo2", OrderId = 1 }; // Should fail due to Article number

        //Act
        var updatedResult = _orderRowRepository.Update(createResult, updatedOrderRow);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        var updatedOrderRow = new OrderRowEntity() { Id = createResult.Id, Quantity = 2, OrderRowPrice = 20, ArticleNumber = "Demo1", OrderId = 1 };

        //Act
        var updatedResult = _orderRowRepository.Update(x => x.Id == createResult.Id, updatedOrderRow);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal(2, updatedResult.Quantity);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        var updatedOrderRow = new OrderRowEntity() { Id = 0, Quantity = 2, OrderRowPrice = 20, ArticleNumber = "Demo2", OrderId = 1 }; // Should fail due to changed Id

        //Act
        var updatedResult = _orderRowRepository.Update(x => x.Id == createResult.Id, updatedOrderRow);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var deletedResult = _orderRowRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(deletedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        //var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var deletedResult = _orderRowRepository.Delete(x => x.Id == orderRow.Id);

        //Assert
        Assert.False(deletedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var deletedResult = _orderRowRepository.Delete(createResult);

        //Assert
        Assert.True(deletedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        //var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var deletedResult = _orderRowRepository.Delete(orderRow);

        //Assert
        Assert.False(deletedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        var createResult = _orderRowRepository.Create(orderRow);

        //Act
        var existResult = _orderRowRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(existResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var orderRow = new OrderRowEntity() { Quantity = 1, OrderRowPrice = 10, ArticleNumber = "Demo1", OrderId = 1 };
        //var createResult = _orderRowRepository.Create(orderRow);


        //Act
        var existResult = _orderRowRepository.Exists(x => x.Id == orderRow.Id);

        //Assert
        Assert.False(existResult);
    }
}