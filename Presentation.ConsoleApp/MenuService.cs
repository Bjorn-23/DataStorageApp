using Business.Dtos;
using Business.Services;
using Infrastructure.Entities;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Presentation.ConsoleApp;

internal class MenuService
{

    private readonly CustomerService _customerService;
    private readonly AddressService _addressService;
    private readonly Customer_AddressService _customer_addressService;

    public MenuService(CustomerService customerService, AddressService addressService, Customer_AddressService customer_addressService)
    {
        _customerService = customerService;
        _addressService = addressService;
        _customer_addressService = customer_addressService;
    }

    internal void MenuStart()
    {
        bool loop = true;

        while (loop)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("----------Choose-an-option----------");
            Console.WriteLine($"{"\n1.",-5} Create a customer");
            Console.WriteLine($"{"\n2.",-5} Create an address");
            Console.WriteLine($"{"\n3.",-5} Create a customer address");
            Console.WriteLine($"{"\n4.",-5} Show customer details");
            Console.WriteLine($"{"\n0",-5} Exit application");
            Console.Write("\nOption: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowCreateCustomerMenu();
                    break;
                case "2":
                    ShowCreateAddressMenu();
                    break;
                case "3":
                    ShowCreateCustomer_AddressMenu();
                    break;
                case "4":
                    ShowCustomerDetails();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        bool ShowCreateCustomerMenu()
        {
            CustomerDto customer = new();

            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("----------Type-in-details-of-new-customer----------");
            Console.Write("\nEmail: ");
            customer.Email = Console.ReadLine()!;
            var exists = _customerService.CustomerExists(customer);
            if (exists)
            {
                Console.WriteLine("\nEmail is already in use, please use a different email.");
                Continue();
                ShowCreateCustomerMenu();
            }
            Console.Write("\nFirst name: ");
            customer.FirstName = Console.ReadLine()!;
            Console.Write("\nLast name: ");
            customer.LastName = Console.ReadLine()!;
            Console.Write("\nPhone number: ");
            customer.PhoneNumber = Console.ReadLine()!;
            Console.Write("\nPassword: ");
            var password = Console.ReadLine()!;
            customer.Password = GenerateSecurePassword(password);

            Console.WriteLine($"\n{customer.FirstName} {customer.LastName}\n{customer.Email}\n{customer.PhoneNumber}\n");
            Console.Write("Do you want to create this Customer? ");
            var customerAnswer = Console.ReadLine()!;
            if (customerAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                var newCustomer = _customerService.CreateCustomer(customer)!;
                if (newCustomer != null)
                {
                    Console.Clear();

                    Console.WriteLine("---------------------------------------------------");
                    Console.WriteLine("---------------New-Customer-Created----------------");
                    Console.WriteLine($"\n{newCustomer.FirstName} {newCustomer.LastName}\n{newCustomer.Email}\n{newCustomer.PhoneNumber}\n");
                    Console.Write("Would you like to Attach an address to this customer? ");
                    var addresAnswer = Console.ReadLine()!;

                    if (addresAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var address = ShowCreateAddressMenu();
                        var result = _customer_addressService.CreateCustomer_Addresses(newCustomer, address);

                        if (result)
                            Console.WriteLine("\nCustomer Address created.");
                        else
                            Console.WriteLine("\nCustomer Address could not be created or already exists.");
                    }
                    else
                        Console.WriteLine("\nYou can add a customer_address later from option 3 in Main menu.");
                }
            }
            else
                Console.WriteLine("Customer could not be created.\nPlease ensure that no fields are empty and try again.");

            Continue();
            return true;
        }

        AddressDto ShowCreateAddressMenu()
        {
            AddressDto address = new();

            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("----------Type-in-details-of-new-address----------");
            Console.Write("\nStreet name: ");
            address.StreetName = Console.ReadLine()!;
            Console.Write("\nPostal code: ");
            address.PostalCode = Console.ReadLine()!;
            Console.Write("\nCity: ");
            address.City = Console.ReadLine()!;
            Console.Write("\nCountry: ");
            address.Country = Console.ReadLine()!;

            Console.WriteLine($"\n{address.StreetName}\n{address.PostalCode}\n{address.City}\n{address.Country}");
            Console.Write("Do you want to create this address? ");
            var answer = Console.ReadLine()!;

            if (answer.Equals ("y", StringComparison.CurrentCultureIgnoreCase))
            {
                var newAddress = _addressService.CreateAddress(address)!;
                if (newAddress != null)
                {
                    Console.WriteLine("The address was created or already exists.");
                    Console.WriteLine($"\n{newAddress.StreetName}\n{newAddress.PostalCode}\n{newAddress.City}\n{newAddress.Country}");
                    Continue();
                    return newAddress;
                }
            }
            else
                Console.WriteLine("Adderss could not be created, please try again.");
            Continue();
            return null!;
        }

        bool ShowCreateCustomer_AddressMenu()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("----------Type-in-details-of-new-customer_address----------");

            CustomerDto customer = new();
            AddressDto address = new();

            Console.Write("Please enter email of customer: ");
            customer.Email = Console.ReadLine()!;

            Console.Write("Please enter Street name of address: ");
            address.StreetName = Console.ReadLine()!;
            Console.Write("Please enter postal code of address: ");
            address.PostalCode = Console.ReadLine()!;

            var result = _customer_addressService.CreateCustomer_Addresses(customer, address);
            if (result)
                Console.WriteLine("Customer Address created.");
            else
                Console.WriteLine("Customer Address could not be created or already exists.");
            Continue();
            return true;
        }

        void ShowCustomerDetails()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("----------------Get-customer-details---------------");

            CustomerDto customer = new();
            

            Console.Write("\nPlease enter email of customer: ");
            customer.Email = Console.ReadLine()!;

            //NEED TO FIX LOGIC AS TO NOT EXPOSE THE ID and Password
            CustomerEntity existingCustomer = _customerService.GetOneCustomer(customer);
            var customerAddresses = _addressService.GetOneAddressWithCustomerId(existingCustomer);

            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("-----------------Customer-details------------------");
            Console.WriteLine($"\nFirst name: {existingCustomer.FirstName}\nLast name: {existingCustomer.LastName}\nEmail: {existingCustomer.Email}\nPhone number: {existingCustomer.PhoneNumber}\n");

            Console.WriteLine($"\nList of addresses associated with {existingCustomer.FirstName} {existingCustomer.LastName}:");

            var i = 1;
            foreach (var address in customerAddresses)
            {
                Console.WriteLine($"\n{i++}{".",-4}{address.StreetName}\n{"",-5}{address.PostalCode}\n{"",-5}{address.City}\n{"",-5}{address.Country}");
            }

            Continue();
        }

    }

    void Continue()
    {
        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    private static string GenerateSecurePassword(string input)
    {
        // Convert the string to bytes
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        // Use a secure random number generator
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            // Create a byte array to store the random bytes
            byte[] randomBytes = new byte[32]; // Adjust the size based on your requirements

            // Fill the byte array with random bytes
            rng.GetBytes(randomBytes);

            // Combine the original bytes with the random bytes
            byte[] combinedBytes = new byte[inputBytes.Length + randomBytes.Length];
            Buffer.BlockCopy(inputBytes, 0, combinedBytes, 0, inputBytes.Length);
            Buffer.BlockCopy(randomBytes, 0, combinedBytes, inputBytes.Length, randomBytes.Length);

            // Use a secure hash function (SHA-256) to generate a fixed-size hash
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

                // Encode the hashed bytes using Base64
                string base64Password = Convert.ToBase64String(hashedBytes);

                return base64Password;
            }
        }
    }
}
