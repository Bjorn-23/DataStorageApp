using Business.Dtos;
using Business.Services;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Presentation.ConsoleApp;

internal class MenuService
{

    private readonly CustomerService _customerService;
    private readonly AddressService _addressService;
    private readonly Customer_AddressService _customer_addressService;
    private readonly UserService _userService;


    public MenuService(CustomerService customerService, AddressService addressService, Customer_AddressService customer_addressService, UserService userService)
    {
        _userService = userService;
        _customerService = customerService;
        _addressService = addressService;
        _customer_addressService = customer_addressService;

    }

    // Main Menu Start
    //-----------------
    internal void MenuStart()
    {
        bool loop = true;

        while (loop)
        {
            Console.Clear();
            Console.WriteLine($"{"",-5}Main Menu - Choose an option");
            string hyphens = new string('-', $"{"", -5 }Main Menu - Choose an option".Length);
            Console.WriteLine(hyphens);
            Console.WriteLine($"{"\n1.",-5} User Menu");
            Console.WriteLine($"{"\n2.",-5} Customer Menu");
            Console.WriteLine($"{"\n3.",-5} Address Menu");
            Console.WriteLine($"{"\n4.",-5} Create a new customer address");
            Console.WriteLine($"{"\n0.",-5} Exit application");
            Console.Write($"\n\n{"",-5}Option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowUserOptionsMenu();
                    break;
                case "2":
                    ShowCustomerOptionsMenu();
                    break;
                case "3":
                    ShowAddressOptionsMenu();
                    break;
                case "4":
                    ShowCreateCustomer_AddressMenu();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }


    // Users Start
    //-----------------
    void ShowUserOptionsMenu()
    {
        bool userLoop = true;

        while (userLoop)
        {
            MenuTemplate("User", "Users");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowCreateUserMenu();
                    break;
                //case "2":
                //    ShowUserDetails();
                //    break;
                //case "3":
                //    ShowAllUsers();
                //    break;
                case "0":
                    userLoop = false;
                    break;
                default:
                    break;
            }

            void ShowCreateUserMenu()
            {
                UserDto user = new();

                SubMenuTemplate("Type in details of new user");

                Console.Write("\nEmail: ");
                user.Email = Console.ReadLine()!;

                Console.Write("\nPassword: ");
                var password = Console.ReadLine()!;

                Console.Write("\nRole: ");
                user.UserRoleName = Console.ReadLine()!;

                var result = _userService.CreateUser(user, password);

                if (result != null)
                {
                    Console.WriteLine($"\nUser: {user.Email} was created\n");
                    Console.Write($"Do you want to create a new Customer associated with {user.Email}? ");
                    var answer = Console.ReadLine()!;

                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ShowCreateCustomerMenu(result);
                    }
                }
                else
                    Console.Write($"User: {user.Email} was not created and/or is already in use.\nPlease try again");
                Continue();
            }
        }
    }

   
  

    // Customers Start
    //-----------------
    void ShowCustomerOptionsMenu()
    {
        bool customerLoop = true;
        while (customerLoop)
        {
            MenuTemplate("Customer", "Customers");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    UserDto user = new();
                    Console.Write("\nType in Email of user to create customer from: ");
                    user.Email = Console.ReadLine()!;
                    ShowCreateCustomerMenu(user);
                    break;
                case "2":
                    ShowCustomerDetails();
                    break;
                case "3":
                    ShowAllCustomers();
                    break;
                case "4":
                    ShowUpdateCustomer();
                    break;
                case "5":
                    ShowDeleteCustomer();
                    break;
                case "0":
                    customerLoop = false;
                    break;
                default:
                    break;
            }
        }
    }

    void ShowCreateCustomerMenu(UserDto user)
    {
        CustomerDto customer = new();
          
        SubMenuTemplate("Type in details of new customer");

        customer.EmailId = user.Email;
        Console.Write("\nFirst name: ");
        customer.FirstName = Console.ReadLine()!;
        Console.Write("\nLast name: ");
        customer.LastName = Console.ReadLine()!;
        Console.Write("\nPhone number: ");
        customer.PhoneNumber = Console.ReadLine()!;

        Console.WriteLine($"\n{customer.FirstName} {customer.LastName}\n{customer.EmailId}\n{customer.PhoneNumber}\n");
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
                Console.WriteLine($"\n{newCustomer.FirstName} {newCustomer.LastName}\n{newCustomer.EmailId}\n{newCustomer.PhoneNumber}\n");
                Console.Write("Would you like to Attach an address to this customer? ");
                var addresAnswer = Console.ReadLine()!;

                if (addresAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                {
                    var address = ShowCreateAddressMenu();
                    var result = _customer_addressService.CreateCustomer_Addresses(newCustomer, address);

                    if (result)
                        Console.WriteLine("\nAddress and Customer-address created.");
                    else
                        Console.WriteLine("\nCustomer address could not be created or already exists.");
                }
                else
                    Console.WriteLine("\nYou can add a customer address from the Main menu.");
            }
            else
                Console.WriteLine("Customer could not be created.\nPlease ensure that a valid User email exists and no fields are empty, then try again.");
        }

        Continue();
    }

    void ShowCustomerDetails()
    {
        CustomerDto customer = new();

        SubMenuTemplate("Get customer details");

        Console.Write("\nPlease enter email of customer: ");
        customer.EmailId = Console.ReadLine()!;

        var customerDetails = _customerService.GetCustomerDetails(customer);
        var customerAddresses = _addressService.GetAddressesWithCustomerId(customerDetails);

        Console.Clear();
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("-----------------Customer-details------------------");

        if (customerDetails != null)
        {
            Console.WriteLine($"\nCustomer: {customerDetails.FirstName} {customerDetails.LastName}.");

            Console.WriteLine($"\nId: {"",-10}{customerDetails.Id}\nFirst name: {"",-2}{customerDetails.FirstName}\nLast name: {"",-3}{customerDetails.LastName}\nEmail: {"",-7}{customerDetails.EmailId}\nPhone number: {"",-0}{customerDetails.PhoneNumber}\nRole: {"",-8}{customerDetails.UserRoleName}\n");

            Console.WriteLine($"\nList of addresses associated with {customerDetails.FirstName} {customerDetails.LastName}:");

            if (customerAddresses != null)
            {
                var i = 1;
                foreach (var address in customerAddresses)
                {
                    Console.WriteLine($"\n{i++}{".",-13}{address.StreetName}\n{"",-14}{address.PostalCode}\n{"",-14}{address.City}\n{"",-14}{address.Country}");
                }
            }
            else
                Console.WriteLine("\nThere are currently no addresses linked to this customer");
        }
        else
            Console.WriteLine($"\nThere are currently no customers linked to {customer.EmailId}");

        Continue();
    }

    void ShowAllCustomers()
    {
        SubMenuTemplate("All current customers");

        var allCustomers = _customerService.GetAll();
        if (allCustomers.Count() >= 1) 
        {
            var i = 1;
            foreach (var customer in allCustomers)
            {
                Console.WriteLine($"\n{i++}{".",-4}First name: {customer.FirstName}\n{"",-5}Last name: {customer.LastName}\n{"",-5}Email: {customer.EmailId}\n{"",-5}Phone number: {customer.PhoneNumber}\n");
            }
        }
        else
            Console.WriteLine("There are currently no customers to display.");

        Continue();
    }

    void ShowUpdateCustomer()
    {
        CustomerDto customer = new();

        SubMenuTemplate("Update customer - Please choose an option");

        Console.WriteLine("\n1. (Update by email).");
        Console.WriteLine("\n2. (Update by ID).");
        Console.WriteLine("\n3. (Update by Phone number).");

        Console.Write("Option: ");
        var answer = Console.ReadLine()!;

        var updateChoice = OptionsSwitch(answer);

        var customerToUpdate = _customerService.GetOneCustomer(updateChoice);

        Console.WriteLine($"\n{customerToUpdate.Id}\n{customerToUpdate.FirstName} {customerToUpdate.LastName}\n{customerToUpdate.EmailId}\n{customerToUpdate.PhoneNumber}\n");
        Console.Write("Is this the customer you wish to update? ");
        var customerAnswer = Console.ReadLine()!;
        if (customerAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        {           
            Console.Write("\n\nFirst name: ");
            customer.FirstName = Console.ReadLine()!;
            Console.Write("\nLast name: ");
            customer.LastName = Console.ReadLine()!;
            Console.Write("\nEmail: ");
            customer.EmailId = Console.ReadLine()!;
            Console.Write("\nPhone number: ");
            customer.PhoneNumber = Console.ReadLine()!;

            Console.WriteLine($"\n{customer.FirstName} {customer.LastName}\n{customer.EmailId}\n{customer.PhoneNumber}\n");
            Console.WriteLine("\nDo you want to update customer with these details?\n(This will also update User with new email and requires your user password.)");
            Console.Write("Continue with update? ");
            var updateAnswer = Console.ReadLine()!;
            if (updateAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                UserDto existingUser = new()
                {
                    Email = customerToUpdate.EmailId
                };

                Console.Write("Password ");
                var password = Console.ReadLine()!; 
                var userResult = _userService.UpdateUser(existingUser, customer, password);

                if (userResult)
                {
                    Console.WriteLine("User updated succesfully");
                    var customerResult = _customerService.UpdateCustomer(customerToUpdate, customer);
                    if (customerResult != null)
                    {
                        Console.WriteLine("Customer updated succesfully");
                    }
                    else
                    {
                        Console.WriteLine("Customer failed to update");

                    }
                }
                else
                    Console.WriteLine("User failed to update");
                Continue();
            }
        }
    }

    void ShowDeleteCustomer()
    {
        SubMenuTemplate("Delete customer - Please choose an option");

        CustomerDto customer = new();

        Console.WriteLine("\nPlease choose an option.");
        Console.WriteLine("\n1. (Delete by email).");
        Console.WriteLine("\n2. (Delete by ID).");
        Console.WriteLine("\n3. (Delete by Phone number).");

        Console.Write("Option: ");
        var answer = Console.ReadLine()!;
        customer = OptionsSwitch(answer);

        var result = _customerService.DeleteCustomer(customer, answer);
        if (result)
        {
            Console.WriteLine($"Id: {customer.Id}\n {customer.FirstName} {customer.LastName}\nEmail: {customer.EmailId}\nPhone Number: {customer.PhoneNumber}Has been deleted.");
        }
        else
            Console.WriteLine($"Customer could not be deleted");

        Continue();
    }


    // Addresses Start
    //-----------------
    void ShowAddressOptionsMenu()
    {
        bool addressloop = true;

        while (addressloop)
        {
            MenuTemplate("Adress", "Adresses");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowCreateAddressMenu();
                    break;
                case "2":
                    ShowAddressDetails();
                    break;
                case "3":
                    ShowAllAddresses();
                    break;
                case "0":
                    addressloop = false;
                    break;
                default:
                    break;
            }
        }
    }

    AddressDto ShowCreateAddressMenu()
    {
        AddressDto address = new();

        SubMenuTemplate("Type in details of new address");

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

        if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
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
            Console.WriteLine("Address could not be created, please try again.");
        Continue();
        ShowAddressOptionsMenu();
        return null!;
    }

    void ShowAddressDetails()
    {
        AddressDto address = new();

        SubMenuTemplate("Get address details");
        Console.Write("\nPlease enter Street name and postal code of Address: \n");

        Console.Write("Street name: ");
        address.StreetName = Console.ReadLine()!;
        Console.Write("Postal Code: ");
        address.PostalCode = Console.ReadLine()!;

        var addressDetails = _addressService.GetOneAddress(address);
        var addressCustomers = _customerService.GetCustomersWithAddressId(addressDetails);

        Console.Clear();
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("------------------Address-details------------------");

        if (addressDetails != null)
        {
            Console.WriteLine($"\nStreet name: {addressDetails.StreetName}\nPostal code: {addressDetails.PostalCode}\nCity: {addressDetails.City}\nPhone number: {addressDetails.Country}\nRole: {addressDetails.Id}\n");

            Console.WriteLine($"\nList of customers associated with {addressDetails.StreetName}, {addressDetails.PostalCode}:");

            if (addressCustomers != null)
            {
                var i = 1;
                foreach (var customer in addressCustomers)
                {
                    Console.WriteLine($"\n{i++}{".",-4}{customer.FirstName} {customer.LastName}\n{"",-5}{customer.EmailId}\n{"",-5}{customer.PhoneNumber}");
                }
            }
            else
                Console.WriteLine("\nThere are currently no customers linked to this Address");
        }
        else
            Console.WriteLine($"\nThere are currently no customers linked to {address.StreetName}, {address.PostalCode}");

        Continue();
    }

    void ShowAllAddresses()
    {
        SubMenuTemplate("All current addresses");

        var allAddresses = _addressService.GetAll();

        var i = 1;
        foreach (var address in allAddresses)
        {
            Console.WriteLine($"\n{i++}{".",-4}{address.StreetName}\n{"",-5}{address.PostalCode}\n{"",-5}{address.City}\n{"",-5}{address.Country}");
        }
        Continue();
    }


    // Customer_Addresses Start
    //-------------------------
    bool ShowCreateCustomer_AddressMenu()
    {
        Console.Clear();
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("----------Type-in-details-of-new-customer_address----------");

        CustomerDto customer = new();
        AddressDto address = new();

        Console.Write("Please enter email of customer: ");
        customer.EmailId = Console.ReadLine()!;

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


    // Misc
    //-----------------
    void Continue()
    {
        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey();
    }

    void MenuTemplate(string singularDto, string pluralDto)
    {
        Console.Clear();
        Console.WriteLine($"{"", -5}{singularDto} Menu - Choose an option");
        string hyphens = new string('-', $"{"",5}{singularDto} Menu - Choose an option".Length);
        Console.WriteLine(hyphens);
        Console.WriteLine($"{"\n1.",-5} Create a new {singularDto}");
        Console.WriteLine($"{"\n2.",-5} Show {singularDto} details");
        Console.WriteLine($"{"\n3.",-5} Show all {pluralDto}");
        Console.WriteLine($"{"\n4.",-5} Update {singularDto}");
        Console.WriteLine($"{"\n5.",-5} Delete {singularDto}");
        Console.WriteLine($"{"\n0.",-5} Go back");
        Console.Write($"\n\n{"", -5}Option: ");
    }

    void SubMenuTemplate(string menuName)
    {
        Console.Clear();
        Console.WriteLine($"{"",-5}{menuName}");
        string hyphens = new string('-', $"{"",5}{menuName}".Length);
        Console.WriteLine(hyphens);
    }

    CustomerDto OptionsSwitch(string answer)
    {
        CustomerDto customer = new CustomerDto();

        switch (answer)
        {
            case "1":
                Console.Write("\nPlease enter email of customer to delete: ");
                customer.EmailId = Console.ReadLine()!;
                break;
            case "2":
                Console.Write("\nPlease enter Id of customer to delete: ");
                customer.Id = Console.ReadLine()!;
                break;
            case "3":
                Console.Write("\nPlease enter Phone number of customer to delete: ");
                customer.PhoneNumber = Console.ReadLine()!;
                break;
            default:
                break;
        }
        return customer;
    }
}
