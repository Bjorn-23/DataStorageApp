using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class CategoryRepository_Tests
{

    private readonly CategoryRepository _categoryRepository;

    public CategoryRepository_Tests()
    {
        var context = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _categoryRepository = new CategoryRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };

        //Act
        var createResult = _categoryRepository.Create(category);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Demo", createResult.CategoryName);
        Assert.Equal(1, createResult.Id);
    }


    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var category = new CategoryEntity() {};

        //Act
        var createResult = _categoryRepository.Create(category);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        //Act
        var getResult = _categoryRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo", getResult.FirstOrDefault()!.CategoryName);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = _categoryRepository.Create(address);

        //Act
        var getResult = _categoryRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        //Act
        var getResult = _categoryRepository.GetAllWithPredicate(x => x.CategoryName == createResult.CategoryName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo", getResult.FirstOrDefault()!.CategoryName);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        CategoryEntity otherCategory = new() { CategoryName = "Test" };

        //Act
        var getResult = _categoryRepository.GetAllWithPredicate(x => x.CategoryName == otherCategory.CategoryName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        //Act
        var getResult = _categoryRepository.GetOne(x => x.CategoryName == createResult.CategoryName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo", getResult.CategoryName);
        Assert.Equal(1, getResult.Id);
    }


    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = _categoryRepository.Create(address);

        //Act
        var getResult = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = createResult.Id, CategoryName = "Test" };

        //Act
        var updatedResult = _categoryRepository.Update(createResult, updatedCategory);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Test", updatedResult.CategoryName);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = 0, CategoryName = "Test" }; //changes Id which should make it fail.

        //Act
        var updatedResult = _categoryRepository.Update(createResult, updatedCategory);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = createResult.Id, CategoryName = "Test" };

        //Act
        var updatedResult = _categoryRepository.Update(x => x.Id == createResult.Id, updatedCategory);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Test", updatedResult.CategoryName);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = 0, CategoryName = "Test" }; //changes Id which should make it fail.

        //Act
        var updatedResult = _categoryRepository.Update(x => x.Id == createResult.Id, updatedCategory);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        //Act
        var updatedResult = _categoryRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = _categoryRepository.Create(category);

        //Act
        var updatedResult = _categoryRepository.Delete(x => x.Id == category.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        //Act
        var updatedResult = _categoryRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = _categoryRepository.Create(category);

        //Act
        var updatedResult = _categoryRepository.Delete(category);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = _categoryRepository.Create(category);

        //Act
        var updatedResult = _categoryRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = _categoryRepository.Create(category);


        //Act
        var updatedResult = _categoryRepository.Exists(x => x.Id == category.Id);

        //Assert
        Assert.False(updatedResult);
    }
}