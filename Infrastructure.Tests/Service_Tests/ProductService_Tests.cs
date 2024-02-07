using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Infrastructure.Tests.Service_Tests;

public class ProductService_Tests
{
    private readonly PriceListRepository _priceListRepository;
    private readonly UserRepository _userRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly UserService _userService;
    private readonly PriceListService _priceListService;
    private readonly UserRoleRepository _userRoleRepository;
    private readonly ProductRepository _productRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;

    public ProductService_Tests()
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
        _productService = new ProductService(_productRepository, _categoryService, _priceListService, _userService);
    }

    [Fact]
    public void CreateProduct_ShouldCreateNewProductInDatabaseAnd_ReturnProduct()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = _priceListService.CreatePriceList(new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" });
        var productRegistration = (new ProductRegistrationDto(){ ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });

        //Act
        var createResult = _productService.CreateProduct(productRegistration);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("DemoProduct1", createResult.ArticleNumber);
    }

    [Fact]
    public void GetProductDisplay_ShouldFetchProductFromDatabaseAnd_ReturnProductRegistrationDto()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = _priceListService.CreatePriceList(new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" });
        var productRegistration = _productService.CreateProduct(new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });

        //Act
        var getResult = _productService.GetProductDisplay(productRegistration);
        
        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("DemoProduct1", getResult.ArticleNumber);
    }

    [Fact]
    public void GetAllProducts_ShouldGetALLExistingProductsFromDatabaseAnd_ReturnThem()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = _priceListService.CreatePriceList(new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" });
        var productRegistration = _productService.CreateProduct(new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });

        //Act
        var getAllResult = _productService.GetAllProducts();

        //Assert
        Assert.NotNull(getAllResult);
        Assert.Equal("DemoProduct1", getAllResult.FirstOrDefault()!.ArticleNumber);
        Assert.NotEmpty(getAllResult);
    }

    [Fact]
    public void UpdateProduct_ShouldUpdateExistingProductInDatabaseAnd_ReturnUpdatedProduct()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = _priceListService.CreatePriceList(new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" });
        var productRegistration = _productService.CreateProduct(new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });

        var updateproducRegistrationForm = new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Test product", Ingress = "", Description = "", CategoryName = "Testproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, };

        //Act
        var updateResult = _productService.UpdateProduct(updateproducRegistrationForm);

        //Assert
        Assert.NotNull(updateResult);
        Assert.Equal("DemoProduct1", updateResult.ArticleNumber);
        Assert.Equal("Test product", updateResult.Title);
        Assert.NotEqual(productRegistration.CategoryId, updateResult.CategoryId);
    }

    [Fact]
    public void UpdateProductStock_ShouldUpdateExistingProductStockInDatabaseAnd_ReturnUpdatedProduct()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = _priceListService.CreatePriceList(new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" });
        var productRegistration = _productService.CreateProduct(new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });

        var updateproducRegistrationForm = new ProductRegistrationDto() { ArticleNumber = productRegistration.ArticleNumber, Stock = -1 }; // Stock is calculated from when OrderRow is updated and is basically plus or minus the wuantity in the orderRow.

        //Act
        var updateResult = _productService.UpdateProductStock(updateproducRegistrationForm);

        //Assert
        Assert.NotNull(updateResult);
        Assert.Equal("DemoProduct1", updateResult.ArticleNumber);
        Assert.Equal(9, updateResult.Stock);
    }

    [Fact]
    public void DeleteProduct_ShouldDeleteExistingProductFromDatabaseAnd_ReturnDeletedProduct()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = _priceListService.CreatePriceList(new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" });
        var productRegistration = _productService.CreateProduct(new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });

        //Act
        var deleteResult = _productService.DeleteProduct(productRegistration);

        var getAllResult = _productService.GetAllProducts();

        //Assert
        Assert.NotNull(deleteResult);
        Assert.Equal("DemoProduct1", deleteResult.ArticleNumber);
        Assert.Empty(getAllResult);
    }
}
