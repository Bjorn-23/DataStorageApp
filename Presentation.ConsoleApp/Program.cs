using Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;
using Infrastructure.Interfaces;
using Presentation.ConsoleApp;
using Business.Services;
using System.Diagnostics;


var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\source\DataStorageApp\Infrastructure\Data\New_DataStorageApp_db.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddDbContext<ProductContext>(x => x.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\source\\DataStorageApp\\Infrastructure\\Data\\New_OrderingSystem_db.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<CustomerService>();

    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddScoped<AddressService>();

    services.AddScoped<ICustomer_AddressRepository, Customer_AddressRepository>();
    services.AddScoped<Customer_AddressService>();
     
    services.AddScoped<IUserRoleRepository, UserRoleRepository>();

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<UserService>();
    services.AddScoped<UserRegistrationService>();
    services.AddScoped<UserRoleService>();

    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<OrderService>();

    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<ProductService>();

    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<CategoryService>();

    services.AddScoped<IPriceListRepository, PriceListRepository>();
    services.AddScoped<PriceListService>();


    services.AddScoped<MenuService>();


}).Build();

try
{
    builder.Start();
   
    var userService = builder.Services.GetRequiredService<UserService>();
    userService.LogoutUsers();

    var menuService = builder.Services.GetRequiredService<MenuService>();
    menuService.MenuStart();

    AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
    {
        userService.LogoutUsers();
    };
}
catch (Exception ex)
{
    Debug.WriteLine("ERROR :: " + ex.Message);
    Console.WriteLine("There was an error while starting the app.");
}


