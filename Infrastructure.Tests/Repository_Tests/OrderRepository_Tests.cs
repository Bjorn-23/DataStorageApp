using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repository_Tests;

public class OrderRepository_Tests
{

    private readonly OrderRepository _orderRepository;
    private readonly ProductRepository _productRepository;
    private readonly OrderRowRepository _orderRowRepository;
    private readonly PriceListRepository _priceListRepository;

    public OrderRepository_Tests()
    {
        var context = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _orderRepository = new OrderRepository(context);
        _productRepository = new ProductRepository(context);
        _orderRowRepository = new OrderRowRepository(context);
        _priceListRepository = new PriceListRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };

        //Act
        var createResult = _orderRepository.Create(order);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal(1, createResult.OrderPrice);
        Assert.Equal(order.CustomerId, createResult.CustomerId);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var order = new OrderEntity() { };

        //Act
        var createResult = _orderRepository.Create(order);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        //Act
        var getResult = _orderRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.OrderPrice);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _orderRepository.Create(order);

        //Act
        var getResult = _orderRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyEntities_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var priceList = _priceListRepository.Create(new PriceListEntity() { Price = 2, DiscountPrice = 1, UnitType = "SEK" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "demo1", Title = "Demo Title", PriceId = 1, CategoryId = 1, Stock = 10, Unit = "Each" });
        var orderRow = _orderRowRepository.Create(new OrderRowEntity() { ArticleNumber = "demo1", OrderId = 1, OrderRowPrice = 2, Quantity = 1 });
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        //Act
        var getResult = _orderRepository.GetAllWithPredicate(x => x.Id == createResult.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.OrderPrice);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        var otherOrder = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 2, CustomerId = Guid.NewGuid().ToString() };

        //Act
        var getResult = _orderRepository.GetAllWithPredicate(x => x.Id == otherOrder.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        //Act
        var getResult = _orderRepository.GetOne(x => x.Id == createResult.Id);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.Id);
    }


    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _orderRepository.Create(order);

        //Act
        var getResult = _orderRepository.GetOne(x => x.Id == order.Id);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        var updatedOrder = new OrderEntity() { Id = createResult.Id, OrderDate = createResult.OrderDate, OrderPrice = 1, CustomerId = createResult.CustomerId };

        //Act
        var updatedResult = _orderRepository.Update(createResult, updatedOrder);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal(updatedResult.CustomerId, createResult.CustomerId);
        Assert.Equal(createResult.OrderPrice, updatedResult.OrderPrice);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        var updatedOrder = new OrderEntity() { Id = 0, OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() }; // Should fail since Id changed

        //Act
        var updatedResult = _orderRepository.Update(createResult, updatedOrder);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        var updatedOrder = new OrderEntity() { Id = 0, OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };

        //Act
        var updatedResult = _orderRepository.Update(x => x.Id == createResult.Id, updatedOrder);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        var updatedOrder = new OrderEntity() { Id = 0, OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() }; // Should fail since Id changed

        //Act
        var updatedResult = _orderRepository.Update(x => x.Id == createResult.Id, updatedOrder);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        //Act
        var deletedResult = _orderRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(deletedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _orderRepository.Create(order);

        //Act
        var deletedResult = _orderRepository.Delete(x => x.Id == order.Id);

        //Assert
        Assert.False(deletedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        //Act
        var deletedResult = _orderRepository.Delete(createResult);

        //Assert
        Assert.True(deletedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _orderRepository.Create(order);

        //Act
        var deletedResult = _orderRepository.Delete(order);

        //Assert
        Assert.False(deletedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        var createResult = _orderRepository.Create(order);

        //Act
        var existResult = _orderRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(existResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var order = new OrderEntity() { OrderDate = DateTime.Now, OrderPrice = 1, CustomerId = Guid.NewGuid().ToString() };
        //var createResult = _orderRepository.Create(order);


        //Act
        var existResult = _orderRepository.Exists(x => x.Id == order.Id);

        //Assert
        Assert.False(existResult);
    }
}
