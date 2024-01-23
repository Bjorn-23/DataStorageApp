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
    //services.AddDbContext<ProductCatalogContext>(x => x.UseSqlServer(@""));


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


    services.AddScoped<MenuService>();


}).Build();

try
{

    builder.Start();

    var menuService = builder.Services.GetRequiredService<MenuService>();

    menuService.MenuStart();
}
catch (Exception ex)
{
    Debug.WriteLine("ERROR :: " + ex.Message);
    Console.WriteLine("There was an error while starting the app.");
}


