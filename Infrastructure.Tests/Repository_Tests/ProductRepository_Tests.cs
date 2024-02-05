using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repository_Tests;

public class ProductRepository_Tests
{

    private readonly ProductRepository _productRepository;
    private readonly PriceListRepository _priceListRepository;
    private readonly CategoryRepository _categoryRepository;

    public ProductRepository_Tests()
    {
        var context = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _productRepository = new ProductRepository(context);
        _priceListRepository = new PriceListRepository(context);
        _categoryRepository = new CategoryRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };

        //Act
        var createResult = _productRepository.Create(product);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("DEMO1BLKNO1", createResult.ArticleNumber);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var product = new ProductEntity() { };

        //Act
        var createResult = _productRepository.Create(product);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 1, DiscountPrice = 1, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demo product" });
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        //Act
        var getResult = _productRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("DEMO1BLKNO1", getResult.FirstOrDefault()!.ArticleNumber);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 1, DiscountPrice = 1, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demo product" });
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        //var createResult = producttRepository.Create(product);

        //Act
        var getResult = _productRepository.GetAll();

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        //Act
        var getResult = _productRepository.GetAllWithPredicate(x => x.ArticleNumber == createResult.ArticleNumber);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo Product 1", getResult.FirstOrDefault()!.Title);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        var otherProduct = new ProductEntity() { ArticleNumber = "TEST1WHI02", Title = "Test Product 2", Ingress = "New Test product, similar to demo product", Description = "lorem ipsum", Unit = "Paír", Stock = 1, PriceId = 2, CategoryId = 1 };

        //Act
        var getResult = _productRepository.GetAllWithPredicate(x => x.ArticleNumber == otherProduct.ArticleNumber);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 1, DiscountPrice = 1, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demo product" });
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        //Act
        var getResult = _productRepository.GetOne(x => x.ArticleNumber == createResult.ArticleNumber);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("DEMO1BLKNO1", getResult.ArticleNumber);
    }


    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        //var createResult = producttRepository.Create(product);

        //Act
        var getResult = _productRepository.GetOne(x => x.ArticleNumber == product.ArticleNumber);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        var updatedProduct = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, slightly better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 5, PriceId = 2, CategoryId = 1 };

        //Act
        var updatedResult = _productRepository.Update(createResult, updatedProduct);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal(5, updatedResult.Stock);
        Assert.Equal(createResult.ArticleNumber, updatedResult.ArticleNumber);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        var updatedProduct = new ProductEntity() { ArticleNumber = "TEST1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, slightly better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 5, PriceId = 2, CategoryId = 1 }; // Should fail because Article number was changed

        //Act
        var updatedResult = _productRepository.Update(createResult, updatedProduct);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        var updatedProduct = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, slightly better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 5, PriceId = 2, CategoryId = 1 };

        //Act
        var updatedResult = _productRepository.Update(x => x.ArticleNumber == createResult.ArticleNumber, updatedProduct);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("New Demo product, slightly better than last years", updatedResult.Ingress);
        Assert.Equal(createResult.ArticleNumber, updatedResult.ArticleNumber);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        var updatedProduct = new ProductEntity() { ArticleNumber = "TEST1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, slightly better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 5, PriceId = 2, CategoryId = 1 }; // Should fail because Article number was changed

        //Act
        var updatedResult = _productRepository.Update(x => x.ArticleNumber == createResult.ArticleNumber, updatedProduct);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);
        //Act
        var updatedResult = _productRepository.Delete(x => x.ArticleNumber == createResult.ArticleNumber);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        //var createResult = _productRepository.Create(product);

        //Act
        var updatedResult = _productRepository.Delete(x => x.ArticleNumber == product.ArticleNumber);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        //Act
        var updatedResult = _productRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        //var createResult = _productRepository.Create(product);

        //Act
        var updatedResult = _productRepository.Delete(product);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        var createResult = _productRepository.Create(product);

        //Act
        var updatedResult = _productRepository.Exists(x => x.ArticleNumber == createResult.ArticleNumber);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var product = new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 };
        //var createResult = _productRepository.Create(product);


        //Act
        var updatedResult = _productRepository.Exists(x => x.ArticleNumber == product.ArticleNumber);

        //Assert
        Assert.False(updatedResult);
    }
}