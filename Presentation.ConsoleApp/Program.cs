using Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Entities;


var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\source\DataStorageApp\Infrastructure\Data\New_DataStorageApp_db.mdf;Integrated Security=True;Connect Timeout=30"));

    //services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();


}).Build();

builder.Start();


var customerRepository = builder.Services.GetRequiredService<ICustomerRepository>();

var result = customerRepository.Create(new CustomerEntity()
{
    Id = Guid.NewGuid(),
    Firstname = "Björn",
    Lastname = "Andersson",
    Email = "bjorn@domain.com",
    Password = "Password123!",
    PhoneNumber = "1234567890",
});

if (result != null)
    Console.WriteLine($"Success!\nNew Customer Created:\nId: {result.Id}\nName: {result.Firstname} {result.Lastname}\nEmail: {result.Email}\n");
else
    Console.WriteLine("Something went wrong, Customer not created");

var addressRepository = builder.Services.GetRequiredService<IAddressRepository>();

var result2 = addressRepository.Create(new AddressEntity()
{
    City = "Lerberget",
    Country = "Sweden",
    PostalCode = "263 54",
    StreetName = "Plöjargränd 143",

});

if (result2 != null)
    Console.WriteLine($"Success!\nNew Address Created:\nId: {result2.Id}\nStreet: {result2.StreetName}\nPostal code: {result2.PostalCode}\nCity: {result2.City}\nCountry: {result2.Country}\n");
else
    Console.WriteLine("Something went wrong, Address not created");
Console.ReadKey();