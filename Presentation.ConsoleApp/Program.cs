using Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;
using Infrastructure.Interfaces;
using Presentation.ConsoleApp;
using Business.Services;


var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\source\DataStorageApp\Infrastructure\Data\New_DataStorageApp_db.mdf;Integrated Security=True;Connect Timeout=30"));
    //services.AddDbContext<ProductCatalogContext>(x => x.UseSqlServer(@""));


    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<CustomerService>();

    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddSingleton<AddressService>();

    services.AddScoped<ICustomer_AddressRepository, Customer_AddressRepository>();
    services.AddScoped<Customer_AddressService>();


    services.AddSingleton<MenuService>();


}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MenuService>();

menuService.MenuStart();



//var customerRepository = builder.Services.GetRequiredService<ICustomerRepository>();
//var customer_AddressRepository = builder.Services.GetRequiredService<ICustomer_AddressRepository>();
//var addressRepository = builder.Services.GetRequiredService<IAddressRepository>();





//var address = new AddressEntity();
//var customer = new CustomerEntity();


//void CreateCustomer()
//{

//}

//customer = customerRepository.Create(new CustomerEntity()
//{
//    Id = Guid.NewGuid(),
//    Firstname = "Björn",
//    Lastname = "Andersson",
//    Email = "bjorn@domain.com",
//    Password = "Password123!",
//    PhoneNumber = "1234567890",
//});

//if (customer != null)
//    Console.WriteLine($"Success!\nNew Customer Created:\nId: {customer.Id}\nName: {customer.Firstname} {customer.Lastname}\nEmail: {customer.Email}\n");
//else
//    Console.WriteLine("Something went wrong, Customer not created");



//address = addressRepository.Create(new AddressEntity()
//{
//    City = "Lerberget",
//    Country = "Sweden",
//    PostalCode = "263 54",
//    StreetName = "Plöjargränd 143",

//});

//if (address != null)
//    Console.WriteLine($"Success!\nNew Address Created:\nId: {address.Id}\nStreet: {address.StreetName}\nPostal code: {address.PostalCode}\nCity: {address.City}\nCountry: {address.Country}\n");
//else
//    Console.WriteLine("Something went wrong, Address not created");



//AddressEntity addressA = new()
//{
//    City = "Lerberget",
//    Country = "Sweden",
//    PostalCode = "263 54",
//    StreetName = "Plöjargränd 143",

//};

//CustomerEntity customerC = new()
//{
//    Id = Guid.NewGuid(),
//    Firstname = "Björn",
//    Lastname = "Andersson",
//    Email = "bjorn@domain.com",
//    Password = "Password123!",
//    PhoneNumber = "1234567890",
//};

//if (addressA != null && customerC != null)
//{
//    var resultC = customerRepository.GetOne(c => c.Email == customerC.Email);
//    var resultA = addressRepository.GetOne(a => a.StreetName == addressA.StreetName);

//    var customer_Address = customer_AddressRepository.Create(new Customer_AddressEntity
//    {
//        AddressId = resultA.Id,
//        CustomerId = resultC.Id
//    });
//}



//CreateCustomer();
//CreateAddress();
//CreateCustomer_Address();

//void CreateCustomer()
//{

//    customer = customerRepository.Create(new CustomerEntity()
//    {
//        Id = Guid.NewGuid(),
//        Firstname = "Björn",
//        Lastname = "Andersson",
//        Email = "bjorn@domain.com",
//        Password = "Password123!",
//        PhoneNumber = "1234567890",
//    });

//    if (customer != null)
//        Console.WriteLine($"Success!\nNew Customer Created:\nId: {customer.Id}\nName: {customer.Firstname} {customer.Lastname}\nEmail: {customer.Email}\n");
//    else
//        Console.WriteLine("Something went wrong, Customer not created");

//}

//void CreateAddress()
//{

//    address = addressRepository.Create(new AddressEntity()
//    {
//        City = "Lerberget",
//        Country = "Sweden",
//        PostalCode = "263 54",
//        StreetName = "Plöjargränd 143",

//    });

//    if (address != null)
//        Console.WriteLine($"Success!\nNew Address Created:\nId: {address.Id}\nStreet: {address.StreetName}\nPostal code: {address.PostalCode}\nCity: {address.City}\nCountry: {address.Country}\n");
//    else
//        Console.WriteLine("Something went wrong, Address not created");
//}

//void CreateCustomer_Address()
//{
//    AddressEntity addressA = new()
//    {
//        City = "Lerberget",
//        Country = "Sweden",
//        PostalCode = "263 54",
//        StreetName = "Plöjargränd 143",

//    };

//    CustomerEntity customerC = new()
//    {
//        Id = Guid.NewGuid(),
//        Firstname = "Björn",
//        Lastname = "Andersson",
//        Email = "bjorn@domain.com",
//        Password = "Password123!",
//        PhoneNumber = "1234567890",
//    };

//    if (addressA != null && customerC != null)
//    {
//        var resultC = customerRepository.GetOne(c => c.Email == customerC.Email);
//        var resultA = addressRepository.GetOne(a => a.StreetName == addressA.StreetName);

//        var customer_Address = customer_AddressRepository.Create(new Customer_AddressEntity
//        {
//            AddressId = resultA.Id,
//            CustomerId = resultC.Id
//        });
//    }

//}



Console.ReadKey();
