using Business.Dtos;
using Business.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Presentation.ConsoleApp;

internal class MenuService(CustomerService customerService, AddressService addressService, Customer_AddressService customer_addressService, UserService userService, UserRegistrationService userRegistrationService)
{

    private readonly CustomerService _customerService = customerService;
    private readonly AddressService _addressService = addressService;
    private readonly Customer_AddressService _customer_addressService = customer_addressService;
    private readonly UserService _userService = userService;
    private readonly UserRegistrationService _userRegistrationService = userRegistrationService;


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
                    ShowUserRegistrationMenu();
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
    void ShowUserRegistrationMenu()
    {
        bool userRegLoop = true;

        while (userRegLoop)
        {       
            Console.Clear();
            Console.WriteLine($"{"",-5}User menu - Choose an option");
            string hyphens = new string('-', $"{"",5}User menu - Choose an option".Length);
            Console.WriteLine(hyphens);
            Console.WriteLine($"{"\n1.",-5} Register new User");
            Console.WriteLine($"{"\n2.",-5} Login User");
            Console.WriteLine($"{"\n3.",-5} Logout User");
            Console.WriteLine($"{"\n4.",-5} Update User (requires user to be logged in)");
            Console.WriteLine($"{"\n5.",-5} Delete User (requires user to be logged in)");
            Console.WriteLine($"{"\n0.",-5} Go back");
            Console.Write($"\n\n{"",-5}Option: ");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    UserCreateMenu();
                    break;
                case "2":
                    UserLoginMenu();
                    break;
                case "3":
                    UserLogoutMenu();
                    break;
                case "4":
                    UserUpdateMenu();
                    break;
                case "5":
                    UserDeleteMenu();
                    break;
                case "0":
                    userRegLoop = false;
                    break;
                default:
                    break;
            }

            void UserCreateMenu()
            {
                UserRegistrationDto user = new();

                SubMenuTemplate("Fill in details of new user");

                Console.Write("First Name: ");
                user.FirstName = Console.ReadLine()!;

                Console.Write("Last Name: ");
                user.LastName = Console.ReadLine()!;

                Console.Write("Phone number: ");
                user.PhoneNumber = Console.ReadLine()!;

                Console.Write("Role: ");
                user.UserRoleName = Console.ReadLine()!;

                Console.Write("Street name: ");
                user.StreetName = Console.ReadLine()!;

                Console.Write("Postal code: ");
                user.PostalCode = Console.ReadLine()!;

                Console.Write("City: ");
                user.City = Console.ReadLine()!;

                Console.Write("Country: ");
                user.Country = Console.ReadLine()!;

                Console.Write("Email: ");
                user.Email = Console.ReadLine()!;

                Console.Write("Password: ");
                user.Password = Console.ReadLine()!;

                SubMenuTemplate("Create user Status");
                Console.WriteLine($"\nDo you wish to create a User with the following information:");

                Console.WriteLine($"\nFirst name: {"",-2}{user.FirstName}\nLast name: {"",-3}{user.LastName}\nEmail: {"",-7}{user.Email}\nPhone number: {"",-0}{user.PhoneNumber}\nRole: {"",-8}{user.UserRoleName}\n");
                Console.WriteLine($"\n{"",-15}{user.StreetName}\n{"",-14}{user.PostalCode}\n{"",-14}{user.City}\n{"",-14}{user.Country}");

                Console.Write("\n(Y)es / (N)o? ");
                var customerAnswer = Console.ReadLine()!;
                if (customerAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                {
                    var result = _userRegistrationService.CreateNewUser(user);
                    SubMenuTemplate("Create user Status");
                    if (result.Item1 != null && result.Item2 != null)
                    {
                        Console.WriteLine("\nUser, Customer and Address created succesfully.\nMake a note of your password for future reference.");
                        Console.WriteLine("Please Login to access your account"); 
                    }
                    else
                    {  
                        Console.WriteLine("\nSomething went wrong, user not created.\nPlease make sure no fields are empty and try again.");
                    }
                }
                else
                {
                    SubMenuTemplate("Create user cancelled");
                    Console.WriteLine("\nUser was not created.");
                }

                PressKeyAndContinue();
            }

            void UserLoginMenu()
            {
                UserDto user = new();

                SubMenuTemplate("Login user");

                Console.Write("Email: ");
                user.Email = Console.ReadLine()!;

                Console.Write("Password: ");
                user.Password = Console.ReadLine()!;

                if (user.Email.IsNullOrEmpty() || user.Password.IsNullOrEmpty())
                {
                    SubMenuTemplate("Login error");
                    Console.WriteLine("\nPlease make sure no fields are blank and try again");
                }
                else
                {
                    var result = _userService.UserLogin(user);
                    SubMenuTemplate("Login status");
                    if (result)
                    {
                        Console.WriteLine($"\n{user.Email} was succesfully logged in.");
                    }
                    else
                        Console.WriteLine("\nLogin attempt failed - password or email not recognized.");
                }

                PressKeyAndContinue();
            }

            void UserLogoutMenu()
            {
                var result = _userService.LogoutUsers();
                SubMenuTemplate("Logout status");
                if (result != null)
                {
                    Console.WriteLine($"\n{result.Email} was succesfully logged out.");
                }
                else
                    Console.WriteLine("\nLogout attempt failed - email not recognized / no users currently logged in.");


                PressKeyAndContinue();
            }

            void UserUpdateMenu()
            {
                UserDto existingUser = new();
                UserDto newUserDetails = new();

                SubMenuTemplate("Update user");

                Console.WriteLine("\nType in email of the user to update.");
                Console.Write("Email: ");
                existingUser.Email = Console.ReadLine()!;

                Console.WriteLine($"\nFill in details to update {existingUser.Email}.");

                Console.Write("Email: ");
                newUserDetails.Email = Console.ReadLine()!;

                Console.Write("Password: ");
                newUserDetails.Password = Console.ReadLine()!;

                Console.Write("Role: ");
                newUserDetails.UserRoleName = Console.ReadLine()!;

                var result = _userService.UpdateUser(existingUser, newUserDetails);
                SubMenuTemplate("Update status");
                if (result != null)
                {
                    Console.WriteLine($"\nId:{"", -12}{result.Id}\n\n{"", -15}Was updated to:\n\nEmail:{"", -9}{result.Email}\n\nIf you made changes to your password please store it in a secure location.");
                }
                else
                {
                    Console.WriteLine($"\n{existingUser.Email} could not be updated!\nPlease make sure {existingUser.Email} is logged in and is a valid email.");
                }

                PressKeyAndContinue();
            }

            void UserDeleteMenu()
            {
                UserDto existingUser = new();

                SubMenuTemplate("Delete user");

                Console.WriteLine("\nType in email of the user to delete.");
                Console.Write("Email: ");
                existingUser.Email = Console.ReadLine()!;
                
                var deleteCheck = _userService.GetOne(existingUser);
                SubMenuTemplate("Delete Status");
                if (deleteCheck != null)
                {
                    Console.WriteLine($"\nId:{"",-12}{deleteCheck.Id}\nEmail:{"",-9}{deleteCheck.Email}\nRole: {"", -9}{deleteCheck.UserRoleName}");
                    Console.Write($"\nIs this the user you wish to delete?\n(Y)es / (N)o: ");
                    var answer = Console.ReadLine()!;
                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var result = _userService.DeleteUser(existingUser);
                        SubMenuTemplate("Delete status");
                        if (result != null)
                        {
                            Console.WriteLine($"\nUser with email: {result.Email} Was deleted!");
                        }
                        else
                        {
                            Console.WriteLine($"\n{existingUser.Email} could not be deleted!\nPlease make sure {existingUser.Email} is logged in and is a valid email.");
                        }                        
                    }
                }
                else
                {
                    Console.WriteLine($"No user with email: {existingUser.Email} was found.\nPlease try again with a valid email.");
                }

                PressKeyAndContinue();
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
                    //ShowCreateCustomerMenu(user);
                    break;
                case "2":
                    ShowCustomerDetails();
                    break;
                case "3":
                    ShowAllCustomers();
                    break;
                case "4":
                    //ShowUpdateCustomer();
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

        //void ShowCreateCustomerMenu(UserDto user)
        //{
        //    CustomerDto customer = new();

        //    SubMenuTemplate("Type in details of new customer");

        //    customer.EmailId = user.Email;
        //    Console.Write("\nFirst name: ");
        //    customer.FirstName = Console.ReadLine()!;
        //    Console.Write("\nLast name: ");
        //    customer.LastName = Console.ReadLine()!;
        //    Console.Write("\nPhone number: ");
        //    customer.PhoneNumber = Console.ReadLine()!;

        //    Console.WriteLine($"\n{customer.FirstName} {customer.LastName}\n{customer.EmailId}\n{customer.PhoneNumber}\n");
        //    Console.Write("Do you want to create this Customer? ");
        //    var customerAnswer = Console.ReadLine()!;
        //    if (customerAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        var newCustomer = _customerService.CreateCustomer(customer)!;
        //        if (newCustomer != null)
        //        {
        //            Console.Clear();

        //            Console.WriteLine("---------------------------------------------------");
        //            Console.WriteLine("---------------New-Customer-Created----------------");
        //            Console.WriteLine($"\n{newCustomer.FirstName} {newCustomer.LastName}\n{newCustomer.EmailId}\n{newCustomer.PhoneNumber}\n");
        //            Console.Write("Would you like to Attach an address to this customer? ");
        //            var addresAnswer = Console.ReadLine()!;

        //            if (addresAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        //            {
        //                var address = ShowCreateAddressMenu();
        //                var result = _customer_addressService.CreateCustomer_Addresses(newCustomer, address);

        //                if (result)
        //                    Console.WriteLine("\nAddress and Customer-address created.");
        //                else
        //                    Console.WriteLine("\nCustomer address could not be created or already exists.");
        //            }
        //            else
        //                Console.WriteLine("\nYou can add a customer address from the Main menu.");
        //        }
        //        else
        //            Console.WriteLine("Customer could not be created.\nPlease ensure that a valid User email exists and no fields are empty, then try again.");
        //    }

        //    PressKeyAndContinue();
        //}

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

            PressKeyAndContinue();
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

            PressKeyAndContinue();
        }

        //void ShowUpdateCustomer()
        //{
        //    CustomerDto customer = new();

        //    SubMenuTemplate("Update customer - Please choose an option");

        //    Console.WriteLine("\n1. (Update by email).");
        //    Console.WriteLine("\n2. (Update by ID).");
        //    Console.WriteLine("\n3. (Update by Phone number).");

        //    Console.Write("Option: ");
        //    var answer = Console.ReadLine()!;

        //    var updateChoice = OptionsSwitch(answer);

        //    var customerToUpdate = _customerService.GetOneCustomer(updateChoice);

        //    Console.WriteLine($"\n{customerToUpdate.Id}\n{customerToUpdate.FirstName} {customerToUpdate.LastName}\n{customerToUpdate.EmailId}\n{customerToUpdate.PhoneNumber}\n");
        //    Console.Write("Is this the customer you wish to update? ");
        //    var customerAnswer = Console.ReadLine()!;
        //    if (customerAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        Console.Write("\n\nFirst name: ");
        //        customer.FirstName = Console.ReadLine()!;
        //        Console.Write("\nLast name: ");
        //        customer.LastName = Console.ReadLine()!;
        //        Console.Write("\nEmail: ");
        //        customer.EmailId = Console.ReadLine()!;
        //        Console.Write("\nPhone number: ");
        //        customer.PhoneNumber = Console.ReadLine()!;

        //        Console.WriteLine($"\n{customer.FirstName} {customer.LastName}\n{customer.EmailId}\n{customer.PhoneNumber}\n");
        //        Console.WriteLine("\nDo you want to update customer with these details?\n(This will also update User with new email and requires your user password.)");
        //        Console.Write("Continue with update? ");
        //        var updateAnswer = Console.ReadLine()!;
        //        if (updateAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            UserDto existingUser = new()
        //            {
        //                Email = customerToUpdate.EmailId
        //            };

        //            Console.Write("Password ");
        //            var password = Console.ReadLine()!;
        //            var userResult = _userService.UpdateUser(existingUser, customer, password);

        //            if (userResult)
        //            {
        //                Console.WriteLine("User updated succesfully");
        //                var customerResult = _customerService.UpdateCustomer(customerToUpdate, customer);
        //                if (customerResult != null)
        //                {
        //                    Console.WriteLine("Customer updated succesfully");
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Customer failed to update");

        //                }
        //            }
        //            else
        //                Console.WriteLine("User failed to update");
        //            PressKeyAndContinue();
        //        }
        //    }
        //}

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

            PressKeyAndContinue();
        }
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
                    PressKeyAndContinue();
                    return newAddress;
                }
            }
            else
                Console.WriteLine("Address could not be created, please try again.");
            PressKeyAndContinue();
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

            PressKeyAndContinue();
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
            PressKeyAndContinue();
        }
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
        PressKeyAndContinue();
        return true;
    }


    // Misc
    //-----------------
    void PressKeyAndContinue()
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
