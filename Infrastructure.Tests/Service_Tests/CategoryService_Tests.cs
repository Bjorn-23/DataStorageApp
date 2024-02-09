using Business.Dtos;
using Business.Factories;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class CategoryService_Tests
{
    private readonly PriceListRepository _priceListRepository;
    private readonly UserRepository _userRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly UserService _userService;
    private readonly PriceListService _priceListService;
    private readonly ProductRepository _productRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly CategoryService _categoryService;
    private readonly UserRoleRepository _userRoleRepository;


    public CategoryService_Tests()
    {
        var productCatalog = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        var dataContext = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _priceListRepository = new PriceListRepository(productCatalog);
        _categoryRepository = new CategoryRepository(productCatalog);
        _productRepository = new ProductRepository(productCatalog);
        _userRoleRepository = new UserRoleRepository(dataContext);
        _userRepository = new UserRepository(dataContext);
        _customerRepository = new CustomerRepository(dataContext);
        _userService = new UserService(_customerRepository, _userRepository);
        _priceListService = new PriceListService(_priceListRepository, _userService);
        _categoryService = new CategoryService(_categoryRepository, _userService);
    }

    [Fact]
    public void GetOrCreateCategory_ShouldGetACategoryFromDatabaseIfitAlreadyExistsElseCreatItAnd_ReturnCategoryEntity()
    {
        //Arrange
        var productRegistration = (new ProductRegistrationDto()
        {
            ArticleNumber = "demo1",
            Title = "Demo product",
            Ingress = "",
            Description = "",
            CategoryName = "Demoproducts",
            Unit = "Each",
            Stock = 10,
            Price = 100,
            Currency = "SEK",
            DiscountPrice = 50,
        });

        //Act
        var getOrCreateResult = _categoryService.GetOrCreateCategory(productRegistration);

        //Assert
        Assert.NotNull( getOrCreateResult );
        Assert.Equal(productRegistration.CategoryName, getOrCreateResult.CategoryName);
        Assert.Equal(1, getOrCreateResult.Id);
    }

    [Fact]
    public void CreateCategory_ShouldCreateANewCategoryInDatabaseAnd_ReturnCategory()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = new CategoryEntity() { CategoryName = "Demo" };

        //Act
        var createResult = _categoryService.CreateCategory(CategoryFactory.Create(category));
      
        //Assert
        Assert.NotNull(createResult);
        Assert.Equal(category.CategoryName, createResult.CategoryName);
    }

    [Fact]
    public void GetCategory_ShouldIfCategoryExistsFetchItFromDatabaseAnd_ReturnCategory()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryService.CreateCategory(new CategoryDto() { CategoryName = "Demo" });

        //Act
        var getResult = _categoryService.GetCategory(category);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(category.CategoryName, getResult.CategoryName);
    }

    [Fact]
    public void GetAllCategoríes_ShouldFetchAllExistingCategoriesFromDatabaseAnd_ReturnListOfCategories()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryService.CreateCategory(new CategoryDto() { CategoryName = "Demo" });

        //Act
        var getAllResult = _categoryService.GetAllCategories();

        //Assert
        Assert.NotNull(getAllResult);
        Assert.Equal(category.CategoryName, getAllResult.FirstOrDefault()!.CategoryName);
    }

    [Fact]
    public void UpdateCategory_ShouldUpdateExistingCategoryInDatabaseWithNewDetailsAnd_ReturnUpdatedCategory()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryService.CreateCategory(new CategoryDto() { CategoryName = "Demo" });
        var updatedCategoryDetails = new CategoryDto() { Id = category.Id, CategoryName = "Test"};
        
        //Act
        var updateResult = _categoryService.UpdateCategory(category, updatedCategoryDetails);

        //Assert
        Assert.NotNull(updateResult);
        Assert.NotEqual(category.CategoryName, updateResult.CategoryName);
        Assert.Equal(category.Id, updateResult.Id);
        Assert.Equal("Test", updateResult.CategoryName);

    }

    [Fact]
    public void DeleteCategory_ShouldIfCategoryExistsDeleteItFromDatabaseAnd_ReturnDeletedCategory()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryService.CreateCategory(new CategoryDto() { CategoryName = "Demo" });

        //Act
        var deleteResult = _categoryService.DeleteCategory(category);
        var getResult = _categoryService.GetAllCategories();

        //Assert
        Assert.NotNull(deleteResult);
        Assert.Equal(category.CategoryName, deleteResult.CategoryName);
        Assert.Empty(getResult);
    }
}
