using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class OrderRowService_Tests
{
    private readonly ProductRepository _productRepository;
    private readonly PriceListRepository _priceListRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly OrderRowRepository _orderRowRepository;
    private readonly UserService _userService;
    private readonly UserRepository _userRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly OrderService _orderService;
    private readonly OrderRepository _orderRepository;
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly PriceListService _priceListService;
    private readonly OrderRowService _orderRowService;
    private readonly UserRoleRepository _userRoleRepository; // Will I use this?

    public OrderRowService_Tests()
    {
        var productCatalog = new ProductCatalog(new DbContextOptionsBuilder<ProductCatalog>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _customerRepository = new CustomerRepository(context);
        _orderRowRepository = new OrderRowRepository(productCatalog);
        _priceListRepository = new PriceListRepository(productCatalog);
        _userRepository = new UserRepository(context);
        _orderRepository = new OrderRepository(productCatalog);
        _userRoleRepository = new UserRoleRepository(context);
        _categoryRepository = new CategoryRepository(productCatalog);
        _productRepository = new ProductRepository(productCatalog);
        _userService = new UserService(_customerRepository, _userRepository);
        _orderService = new OrderService(_orderRepository, _customerRepository, _userRepository, _userService);
        _categoryService = new CategoryService(_categoryRepository, _userService);
        _priceListService = new PriceListService(_priceListRepository, _userService);
        _productService = new ProductService(_productRepository, _categoryService, _priceListService, _userService);
        _orderRowService = new OrderRowService(_orderRowRepository, _productService, _orderService, _customerRepository);
    }

    [Fact]
    public void CreateOrderRow_ShouldCreateNewOrderRowInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var newOrder = _orderService.CreateOrder();
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });

        //Act
        var orderRow = _orderRowService.CreateOrderRow(new OrderRowDto() { ArticleNumber = product.ArticleNumber, Id = 1, OrderId = newOrder.Id, Quantity = 2, OrderRowPrice = 10 });

        //Assert
        Assert.NotNull(orderRow);
        Assert.Equal(newOrder.Id, orderRow.OrderId);
        Assert.Equal(product.ArticleNumber, orderRow.ArticleNumber);
    }

    [Fact]
    public void GetAllOrderRows_ShouldGetAllOrderRowsFromDatabaseAnd_ReturnThem()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var newOrder = _orderService.CreateOrder();
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });
        var orderRow = _orderRowService.CreateOrderRow(new OrderRowDto() { ArticleNumber = product.ArticleNumber, Id = 1, OrderId = newOrder.Id, Quantity = 2, OrderRowPrice = 10 });

        //Act
        var getResult = _orderRowService.GetAllOrderRows();

        //Assert
        Assert.NotNull(getResult);
        Assert.NotEmpty(getResult);
    }

    [Fact]
    public void UpdateOrderRow_ShouldUpdateOneOrderRowInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var newOrder = _orderService.CreateOrder();
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });
        var orderRow = _orderRowService.CreateOrderRow(new OrderRowDto() { ArticleNumber = product.ArticleNumber, Id = 1, OrderId = newOrder.Id, Quantity = 2 });

        var updatedOrderRowDetails = new OrderRowDto() { ArticleNumber = orderRow.ArticleNumber, Id = orderRow.Id, OrderId = orderRow.OrderId, Quantity = 3 };

        //Act
        var updateResult = _orderRowService.UpdateOrderRow(updatedOrderRowDetails);

        //Assert
        Assert.NotNull(updateResult);
        Assert.Equal(3 ,updateResult.Quantity);
        Assert.Equal(15, updateResult.OrderRowPrice);
    }

    [Fact]
    public void DeleteOrderRow_ShouldDeleteOneOrderRowInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var newOrder = _orderService.CreateOrder();
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });
        var orderRow = _orderRowService.CreateOrderRow(new OrderRowDto() { ArticleNumber = product.ArticleNumber, Id = 1, OrderId = newOrder.Id, Quantity = 2 });

        //Act
        var deleteResult = _orderRowService.DeleteOrderRow(orderRow);

        //Assert
        Assert.NotNull(deleteResult);
        Assert.Equal(orderRow.Id, deleteResult.Id);
    }

    [Fact]
    public void GetPriceOrDiscountPrice_ShouldChooseDiscountPriceIfItIsntZeroElseChoosePriceAnd_ReturnDecimal()
    {
        //Arrange
        var productRegistration = (new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 50, });
                
        //Act
        var priceResult = _orderRowService.GetPriceOrDiscountPrice(productRegistration);

        //Assert
        Assert.Equal(productRegistration.DiscountPrice, priceResult);
        Assert.NotEqual(productRegistration.Price, priceResult);
    }

    [Fact]
    public void GetPriceOrDiscountPrice_ShouldChoosePriceSinceDiscountPriceIsZeroAnd_ReturnDecimal()
    {
        //Arrange
        var productRegistration = (new ProductRegistrationDto() { ArticleNumber = "DemoProduct1", Title = "Demo product", Ingress = "", Description = "", CategoryName = "Demoproducts", Unit = "Each", Stock = 10, Price = 100, Currency = "SEK", DiscountPrice = 0, });

        //Act
        var priceResult = _orderRowService.GetPriceOrDiscountPrice(productRegistration);

        //Assert
        Assert.NotEqual(productRegistration.DiscountPrice, priceResult);
        Assert.Equal(productRegistration.Price, priceResult);
    }

    [Fact]
    public void GetOrderId_ShouldGetExistingOrderIdFromDatabaseOrCreateNewOneAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var newOrder = _orderService.CreateOrder();
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });
        var orderRow = _orderRowService.CreateOrderRow(new OrderRowDto() { ArticleNumber = product.ArticleNumber, Id = 1, OrderId = newOrder.Id, Quantity = 2 });
        
        //Act
        var orderResult = _orderRowService.GetOrderId(orderRow);

        //Assert
        Assert.Equal(newOrder.Id, orderResult.Id);
      }

    [Fact]
    public void GetOrderId_ShouldCreateNewOrderInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        //var newOrder = _orderService.CreateOrder(); // Leave commented out so that its forced to create a new Order.
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });
        var orderRow = new OrderRowDto() { ArticleNumber = product.ArticleNumber, Id = 1, Quantity = 2 };

        //Act
        var orderResult = _orderRowService.GetOrderId(orderRow);

        //Assert
        Assert.Equal(1, orderResult.Id);
    }

}
