using Business.Dtos;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class OrderService_Tests
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
    private readonly UserRoleRepository _userRoleRepository;

    public OrderService_Tests()
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
        _orderService = new OrderService(_orderRepository,_customerRepository, _userRepository, _userService); 
    }

    [Fact]
    public void GetActiveUser_ShouldGetActiveUserFromDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });

        //Act
        var activeUSer = _orderService.GetActiveUser();

        //Assert
        Assert.True(activeUSer.IsActive);
    }

    [Fact]
    public void CreateOrder_ShouldCreateNewOrderInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var activeUSer = _orderService.GetActiveUser();

        //Act
        var newOrder = _orderService.CreateOrder();

        //Assert
        Assert.NotNull(newOrder);
        Assert.Equal(activeUser.Id, newOrder.CustomerId);
    }

    [Fact]
    public void GetUsersOrder_ShouldGetActiveUsersOrderInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var activeUSer = _orderService.GetActiveUser();
        var newOrder = _orderService.CreateOrder();

        //Act
        var getResult = _orderService.GetUsersOrder();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(activeUser.Id, getResult.CustomerId);
    }

    [Fact]
    public void GetOrderDetails_ShouldGetActiveUsersOrderInDatabaseAnd_ReturnItWithOrderRowsAndProducts()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var user = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var customer = _customerRepository.Create(new CustomerEntity() { EmailId = user.Email, FirstName = "Björn", LastName = "Andersson", Id = user.Id, PhoneNumber = "0789456123" });
        var newOrder = _orderService.CreateOrder();
        var priceList = _priceListRepository.Create(new PriceListEntity() { Id = 1, Price = 10, DiscountPrice = 5, UnitType = "SEK" });
        var Category = _categoryRepository.Create(new CategoryEntity() { Id = 1, CategoryName = "Demoproducts" });
        var product = _productRepository.Create(new ProductEntity() { ArticleNumber = "DEMO1BLKNO1", Title = "Demo Product 1", Ingress = "New Demo product, so much better than last years", Description = "lorem ipsum", Unit = "Each", Stock = 10, PriceId = 1, CategoryId = 1 });
        var orderRow = _orderRowRepository.Create(new OrderRowEntity() { ArticleNumber = product.ArticleNumber, Id = 1, OrderId = newOrder.Id, Quantity = 2, OrderRowPrice = 10 });


        //Act
        var getDetailsResult = _orderService.GetOrderDetails();

        //Assert
        Assert.NotNull(getDetailsResult.order);
        Assert.NotNull(getDetailsResult.orderRows);
        Assert.NotNull(getDetailsResult.products);
        Assert.NotNull(getDetailsResult.customer);
        Assert.Equal(user.Id, getDetailsResult.customer.Id);
    }

    [Fact]
    public void UpdateOrder_ShouldUpdateOrderInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var activeUSer = _orderService.GetActiveUser();
        var newOrder = _orderService.CreateOrder();

        var updatedOrderDetails = new OrderDto() { Id = newOrder.Id, CustomerId = newOrder.CustomerId, OrderDate = newOrder.OrderDate, OrderPrice = 500 };

        //Act
        var getResult = _orderService.UpdateOrder(updatedOrderDetails);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(activeUser.Id, getResult.CustomerId);
        Assert.Equal(500, getResult.OrderPrice);
    }

    [Fact]
    public void DeleteOrder_ShouldDeleteOrderInDatabaseAnd_ReturnIt()
    {
        //Arrange
        var userRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Admin" });
        var activeUser = _userService.CreateUser(new UserDto() { Id = "1", Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "2", IsActive = true, UserRoleId = 1 });
        var activeUSer = _orderService.GetActiveUser();
        var newOrder = _orderService.CreateOrder();

        //Act
        var getResult = _orderService.DeleteOrder(newOrder);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(activeUser.Id, getResult.CustomerId);
    }
}
