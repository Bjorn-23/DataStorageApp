using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class CategoryRepository_Tests : BaseRepository<CategoryEntity, ProductCatalog>, ICategoryRepository
{

    private readonly ProductCatalog _context;

    public CategoryRepository_Tests() : base(new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
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
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };

        //Act
        var createResult = categoryRepository.Create(category);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Demo", createResult.CategoryName);
        Assert.Equal(1, createResult.Id);
    }


    [Fact]
    public void CreateShould_NotCreateOneUserInDatabase_ReturnNulll()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() {};

        //Act
        var createResult = categoryRepository.Create(category);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllUsersFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        //Act
        var getResult = categoryRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo", getResult.FirstOrDefault()!.CategoryName);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoUsersInDatabase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = categoryRepository.Create(address);

        //Act
        var getResult = categoryRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyUserWithTheSuppliedRoleExists_ReturnThatUserFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        //Act
        var getResult = categoryRepository.GetAllWithPredicate(x => x.CategoryName == createResult.CategoryName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo", getResult.FirstOrDefault()!.CategoryName);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoUserWithTheSuppliedRoleExists_ReturnEmptyList()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        CategoryEntity otherCategory = new() { CategoryName = "Test" };

        //Act
        var getResult = categoryRepository.GetAllWithPredicate(x => x.CategoryName == otherCategory.CategoryName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfUserExists_ReturnOneUserFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        //Act
        var getResult = categoryRepository.GetOne(x => x.CategoryName == createResult.CategoryName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Demo", getResult.CategoryName);
        Assert.Equal(1, getResult.Id);
    }


    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = categoryRepository.Create(address);

        //Act
        var getResult = categoryRepository.GetOne(x => x.CategoryName == category.CategoryName);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = createResult.Id, CategoryName = "Test" };

        //Act
        var updatedResult = categoryRepository.Update(createResult, updatedCategory);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Test", updatedResult.CategoryName);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateShould_FailToUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = 0, CategoryName = "Test" }; //changes Id which should make it fail.

        //Act
        var updatedResult = categoryRepository.Update(createResult, updatedCategory);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_Should_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = createResult.Id, CategoryName = "Test" };

        //Act
        var updatedResult = categoryRepository.Update(x => x.Id == createResult.Id, updatedCategory);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Test", updatedResult.CategoryName);
        Assert.Equal(createResult.Id, updatedResult.Id);
    }

    [Fact]
    public void UpdateWithPredicate_Should_NotUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        CategoryEntity updatedCategory = new() { Id = 0, CategoryName = "Test" }; //changes Id which should make it fail.

        //Act
        var updatedResult = categoryRepository.Update(x => x.Id == createResult.Id, updatedCategory);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        //Act
        var updatedResult = categoryRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = categoryRepository.Create(category);

        //Act
        var updatedResult = categoryRepository.Delete(x => x.Id == category.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        //Act
        var updatedResult = categoryRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = categoryRepository.Create(category);

        //Act
        var updatedResult = categoryRepository.Delete(category);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        var createResult = categoryRepository.Create(category);

        //Act
        var updatedResult = categoryRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        //Arrange
        var categoryRepository = new CategoryRepository_Tests();
        var category = new CategoryEntity() { CategoryName = "Demo" };
        //var createResult = categoryRepository.Create(category);


        //Act
        var updatedResult = categoryRepository.Exists(x => x.Id == category.Id);

        //Assert
        Assert.False(updatedResult);
    }
}