using Business.Dtos;
using Business.Factories;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class PriceListService_Tests
{
    private readonly PriceListRepository _priceListRepository;
    private readonly UserRepository _userRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly UserService _userService;
    private readonly PriceListService _priceListService;
    private readonly UserRoleRepository _userRoleRepository;
    private readonly ProductRepository _productRepository;
    private readonly CategoryRepository _categoryRepository;

    public PriceListService_Tests()
    {
        var productCatalog = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        var dataContext = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _priceListRepository = new PriceListRepository(productCatalog);
        _userRoleRepository = new UserRoleRepository(dataContext);
        _userRepository = new UserRepository(dataContext);
        _customerRepository = new CustomerRepository(dataContext);
        _userService = new UserService(_customerRepository, _userRepository);
        _priceListService = new PriceListService(_priceListRepository, _userService);
        _productRepository = new ProductRepository(productCatalog);
        _categoryRepository = new CategoryRepository(productCatalog);
    }

    [Fact]
    public void GetOrCreatePriceList_ShouldCreateAPriceListOrGetItFromTheDatabaseIfItAlreadyExistsAnd_ReturnPriceList()
    {
        //Arrange
        var priceList = new ProductRegistrationDto() { Price = 200, DiscountPrice = 100, Currency = "SEK" };
        var createdPriceList = _priceListService.GetOrCreatePriceList(priceList);

        var getPriceList = new ProductRegistrationDto() { Price = createdPriceList.Price, DiscountPrice = createdPriceList.DiscountPrice, Currency = createdPriceList.UnitType };

        //Act
        var getResult = _priceListService.GetOrCreatePriceList(getPriceList);

        //Assert
        Assert.NotNull(createdPriceList);
        Assert.NotNull(getResult);
        Assert.Equal(createdPriceList.Price, getResult.Price);
        Assert.Same(createdPriceList, getResult);
    }

    [Fact]
    public void CreatePriceList_ShouldCreateAPriceListInDatabaseAnd_ReturnPriceList()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var priceList = new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" };

        //Act       
        var createdPriceList = _priceListService.CreatePriceList(priceList);

        //Assert
        Assert.NotNull(createdPriceList);
        Assert.Equal(createdPriceList.Price, priceList.Price);
    }

    [Fact]
    public void GetPriceList_ShouldGetExistingPriceListFromDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" };
        var createdPriceList = _priceListService.CreatePriceList(priceList);
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DemoProduct1", Title = "Demo product", CategoryId = 1, PriceId = 1, Stock = 1, Unit = "Each" });
          
        //Act
        var getResult = _priceListService.GetPriceList(createdPriceList);

        //Assert
        Assert.NotNull(createdPriceList);
        Assert.NotNull(getResult);
        Assert.Equal(createdPriceList.Price, getResult.Price);
    }

    [Fact]
    public void GetAllPriceList_ShouldGetAllExistingPriceListFromDatabaseAnd_ReturnThem()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var category = _categoryRepository.Create(new CategoryEntity() { CategoryName = "Demo" });
        var priceList = new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" };
        var createdPriceList = _priceListService.CreatePriceList(priceList);
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DemoProduct1", Title = "Demo product", CategoryId = 1, PriceId = 1, Stock = 1, Unit = "Each" });

        //Act
        var getResult = _priceListService.GetAllPriceLists();

        //Assert
        Assert.NotNull(createdPriceList);
        Assert.NotNull(getResult);
        Assert.NotEmpty(getResult);
        Assert.IsAssignableFrom<IEnumerable<PriceListDto>>(getResult);
    }

    [Fact]
    public void UpdatePriceList_ShouldUpdateExistingPriceListInDatabaseAnd_ReturnUpdatedItem()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var priceList = new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" };
        var createdPriceList = _priceListService.CreatePriceList(priceList);
        var updatedPriceList = new PriceListDto() { Id = priceList.Id, Price = 300, DiscountPrice = priceList.DiscountPrice, UnitType = priceList.UnitType };

        //Act
        var updatedResult = _priceListService.UpdatePriceList(priceList, updatedPriceList);

        //Assert
        Assert.NotNull(createdPriceList);
        Assert.NotNull(updatedResult);
        Assert.Equal(createdPriceList.Id, updatedResult.Id);
        Assert.NotEqual(createdPriceList.Price, updatedResult.Price);
    }

    [Fact]
    public void DeletePriceList_ShouldDeleteExistingPriceListInDatabaseAnd_ReturnDeleteItem()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Email = "bjorn@domain.com", Password = "Bytmig123!", IsActive = true, UserRoleId = 1 });
        var priceList = new PriceListDto() { Price = 200, DiscountPrice = 100, UnitType = "SEK" };
        var createdPriceList = _priceListService.CreatePriceList(priceList);

        //Act
        var deletedResult = _priceListService.DeletePriceList(priceList);

        //Assert
        Assert.NotNull(createdPriceList);
        Assert.NotNull(deletedResult);
        Assert.Equal(createdPriceList.Id, deletedResult.Id);
    }
}
