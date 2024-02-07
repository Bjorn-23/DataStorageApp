using Business.Dtos;
using Business.Factories;
using Business.Services;
using Infrastructure.Entities;

namespace Presentation.ConsoleApp;

internal class MenuService(CustomerService customerService, AddressService addressService, Customer_AddressService customer_addressService, UserService userService, UserRegistrationService userRegistrationService, OrderService orderService, ProductService productService, OrderRowService orderRowService, PriceListService priceListService, CategoryService categoryService, UserRoleService userRoleService)
{

    private readonly CustomerService _customerService = customerService;
    private readonly AddressService _addressService = addressService;
    private readonly Customer_AddressService _customer_addressService = customer_addressService;
    private readonly UserService _userService = userService;
    private readonly UserRegistrationService _userRegistrationService = userRegistrationService;
    private readonly OrderService _orderService = orderService;
    private readonly ProductService _productService = productService;
    private readonly OrderRowService _orderRowService = orderRowService;
    private readonly PriceListService _priceListService = priceListService;
    private readonly CategoryService _categoryService = categoryService;
    private readonly UserRoleService _userRoleService = userRoleService;

    internal void MenuStart()
    {
        bool loop = true;

        while (loop)
        {
            string title = "Björn's Super Store";
            string[] menuItems = {
                "Login User",
                "Create User",
                "Webshop",
                "User Settings Menu",
                "Product Menu" };
            string exit = "Exit application and Logout User";
            var option = MenuTemplate(menuItems, title, exit);

            switch (option)
            {
                case "1":
                    ShowUserLoginMenu();
                    break;
                case "2":
                    ShowUserCreateMenu();
                    break;
                case "3":
                    ShowWebShopMenu();
                    break;
                case "4":
                    ShowUserSettingsMenu();
                    break;
                case "5":
                    ShowProductMenu();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        void ShowUserLoginMenu()
        {
            UserDto user = new();

            SubMenuTemplate("Login user");
            CWBreak("Email: ");
            user.Email = Console.ReadLine()!;
            CW("Password: ");
            user.Password = Console.ReadLine()!;

            SubMenuTemplate("Login status");
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                CWLBreak("Error, user could not be logged in - Please make sure no fields are blank and try again");
            }
            else
            {
                var result = _userService.UserLogin(user);
                SubMenuTemplate("Login status");
                if (result)
                {
                    CWLBreak($"{user.Email} was succesfully logged in.");
                }
                else
                    CWLBreak("Login attempt failed - password or email not recognized.");
            }

            PressKeyAndContinue();
        }

        void ShowUserCreateMenu()
        {
            UserRegistrationDto user = new();

            SubMenuTemplate("Fill in details of new user");

            CWBreak("First Name: ");
            user.FirstName = Console.ReadLine()!;
            CW("Last Name: ");
            user.LastName = Console.ReadLine()!;
            CW("Phone number: ");
            user.PhoneNumber = Console.ReadLine()!;
            CW("Role: ");
            user.UserRoleName = Console.ReadLine()!;
            CW("Street name: ");
            user.StreetName = Console.ReadLine()!;
            CW("Postal code: ");
            user.PostalCode = Console.ReadLine()!;
            CW("City: ");
            user.City = Console.ReadLine()!;
            CW("Country: ");
            user.Country = Console.ReadLine()!;
            CW("Email: ");
            user.Email = Console.ReadLine()!;
            CW("Password: ");
            user.Password = Console.ReadLine()!;

            SubMenuTemplate("Create user Status");            
            PrintUserToBeCreated(user);
            var customerAnswer = Question("Do you wish to create a User with the above information:");

            if (AnswerIsYes(customerAnswer))
            {
                var result = _userRegistrationService.CreateNewUser(user);
                SubMenuTemplate("Create user Status");
                if (result.Item1 != null && result.Item2 != null)
                {
                    CWL("User, Customer and Address created succesfully!");
                    CWL("Make a note of your password for future reference.");
                    CWLBreak("Please Login to access your account");
                }
                else
                {
                    CWL("Something went wrong, user not created.");
                    CWL("Please make sure no fields are empty and try again.");
                }
            }
            else
            {
                CWLBreak("User was not created.");
            }

            PressKeyAndContinue();
        }

        void ShowWebShopMenu()
        {
            bool webShopLoop = true;

            while (webShopLoop)
            {
                string title = "Webshop menu - Choose an option";
                string[] menuItems = {
                    "Show Order details",
                    "Create Order",
                    "Delete Order",
                    "Add product to Order",
                    "Show Order rows", "Update Order Rows",
                    "Delete Order Rows"};
                string exit = "Go back to previous menu";
                var option = MenuTemplate(menuItems, title, exit);

                switch (option)
                {
                    case "1":
                        ShowOrderDetailsMenu();
                        break;
                    case "2":
                        ShowCreateOrderMenu();
                        break;
                    case "3":
                        ShowDeleteOrderMenu();
                        break;
                    case "4":
                        ShowAddProductToOrderMenu();
                        break;
                    case "5":
                        ShowAllOrderRowMenu();
                        break;
                    case "6":
                        ShowUpdateOrderRow();
                        break;
                    case "7":
                        ShowDeleteOrderRow();
                        break;
                    case "0":
                        webShopLoop = false;
                        break;
                    default:
                        break;
                }
            }

            void ShowOrderDetailsMenu()
            {
                var result = _orderService.GetOrderDetails();

                SubMenuTemplate("Order details:");
                if (result.customer != null && result.products != null && result.orderRows != null && result.order != null)
                {
                    PrintCustomer(result.customer);
                    PrintOrder(result.order);
                    CWLBreak("Order rows:");
                    CWL($"-----------");

                    foreach (var orderRow in result.orderRows)
                    {
                        foreach (var product in result.products.Where(x => x.ArticleNumber == orderRow.ArticleNumber))
                        {
                            var price = product.DiscountPrice >=1 ? product.DiscountPrice : product.Price;
                            PrintOrderRowLoop(orderRow, product, price);
                        }
                    }
                }
                else
                    CWLBreak("User not logged in, or no orders created.");

                PressKeyAndContinue();
            }

            void ShowCreateOrderMenu()
            {
                var order = _orderService.CreateOrder();
                SubMenuTemplate("Created Order details");
                if (order != null)
                {
                    PrintOrder(order);  
                }
                else
                    CWLBreak("User not logged in, or order already created.");

                PressKeyAndContinue();
            }

            void ShowDeleteOrderMenu()
            {
                var order = _orderService.GetUsersOrder();
                SubMenuTemplate("Current Order details");
                if (order != null)
                {
                    CWLBreak("Be advised that any Order Rows associated with the order will also be deleted!");
                    PrintOrder(order);

                    var answer = Question("Are you sure you wish to delete your current order?");
                    if (AnswerIsYes(answer))
                    {
                        var deletedOrder = _orderService.DeleteOrder(order);
                        SubMenuTemplate("Delete Status");
                        if (deletedOrder != null)
                        {
                            PrintOrder(deletedOrder);
                            CWLBreak("Order deleted:");
                        }
                    }
                    else
                        CWLBreak("Order will not be deleted.");
                }
                else
                    CWLBreak("No Orders associated with user, or no user logged in.");

                PressKeyAndContinue();
            }

            void ShowAddProductToOrderMenu()
            {
                var orderRow = new OrderRowDto();

                var products = _productService.GetAllProducts();
                SubMenuTemplate("Create Order row");
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        PrintProductDetails(product);
                    }
                    CWLBreak("Fill in article number and quantity of the product you want to purchase.\n");

                    CW("Article Number*: ");
                    orderRow.ArticleNumber = Console.ReadLine()!;

                    CW("Quantity*: ");
                    var quantityResult = int.TryParse(Console.ReadLine()!, out int quantity);
                    SubMenuTemplate("Order row status:");
                    if (quantityResult && quantity >= 1)
                    {
                        orderRow.Quantity = quantity;

                        var newOrderRow = _orderRowService.CreateOrderRow(orderRow);
                        if (newOrderRow != null)
                        {
                            SubMenuTemplate("New Order row created:");
                            PrintOrderRow(newOrderRow);
                        }
                        else
                            CWLBreak($"Order row could not be created or already exists! If you wish to add more items to your order the use update order rows instead.");
                    }
                    else
                        CWLBreak("Order row could not be created, please try again.");
                }
                else
                    CWLBreak("There are curently no products to purchase, create a new product from the 'Create Product Menu.'");

                PressKeyAndContinue();
            }

            void ShowAllOrderRowMenu()
            {
                var orderRows = _orderRowService.GetAllOrderRows();
                SubMenuTemplate("All Order rows:");
                if (orderRows.Any())
                {
                    foreach (var orderRow in orderRows)
                    {
                        PrintOrderRow(orderRow);
                    }
                }
                else
                    CWLBreak("No Order rows currently associated with user");

                PressKeyAndContinue();
            }

            void ShowUpdateOrderRow()
            {
                var updatedOrderRow = new OrderRowDto();

                var orderRows = _orderRowService.GetAllOrderRows();
                SubMenuTemplate("Update Order row:");
                foreach (var orderRow in orderRows)
                {
                    PrintOrderRow(orderRow);
                }

                Console.WriteLine("Please fill in Id of order to update:");
                CWBreak("Id: ");
                var orderRowId = int.TryParse(Console.ReadLine()!, out int id);
                if (orderRowId || id >= 1)
                {
                    updatedOrderRow.Id = id;

                    CW("Fill in updated quantity for order row: ");
                    var orderRowQuantity = int.TryParse(Console.ReadLine()!, out int quantity);
                    if (orderRowQuantity || quantity >= 1)
                    {
                        updatedOrderRow.Quantity = quantity;
                        var result = _orderRowService.UpdateOrderRow(updatedOrderRow);
                        if (result != null)
                        {
                            SubMenuTemplate("Order row updated to: ");
                            PrintOrderRow(updatedOrderRow);
                        }
                    }
                }
                else
                    CWLBreak("Could not find order row, please try again.");

                PressKeyAndContinue();
            }

            void ShowDeleteOrderRow()
            {
                var order = new OrderRowDto();
                var orderRows = _orderRowService.GetAllOrderRows();
                SubMenuTemplate("All order rows:");
                if (orderRows.Any())
                {
                    foreach (var orderRow in orderRows)
                    {
                        PrintOrderRow(orderRow);
                    }

                    CWLBreak("Type in Id of the order row to delete: ");
                    CWBreak("Id: ");
                    var answer = int.TryParse(Console.ReadLine()!, out int Id);
                    if (answer)
                    {
                        order.Id = Id;

                        var deletedOrderRow = _orderRowService.DeleteOrderRow(order);
                        SubMenuTemplate("Delete Status");
                        if (deletedOrderRow != null)
                        {
                            PrintOrderRow(deletedOrderRow);
                            CWLBreak("Order row deleted:");

                        }
                        else
                            CWLBreak("Something went wrong, order row was not be deleted.");
                    }
                    else
                        CWLBreak("Something went wrong, no order row by that id.");
                }
                else
                    CWLBreak("No order rows currently associated with user");

                PressKeyAndContinue();
            }
        }

        void ShowUserSettingsMenu() // Only this one left! No biggie...
        {
            bool userMenuLoop = true;

            while (userMenuLoop)
            {
                string title = "User settings menu";
                string[] menuItems = {
                    "Show User options",
                    "Show Customer options",
                    "Show Address options",
                    "Show Customer_Address Options" };
                string exit = "Go back to previous menu";
                var option = MenuTemplate(menuItems, title, exit);

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
                        ShowCustomer_AddressOptions();
                        break;
                    case "0":
                        userMenuLoop = false;
                        break;
                    default:
                        break;
                }
                void ShowUserOptionsMenu()
                {
                    bool userRegLoop = true;

                    while (userRegLoop)
                    {
                        string title = "User menu";
                        string[] menuItems = {
                        "Register new User (creates user, customer and address)",
                        "Login User",
                        "Logout User",
                        "Show User details (requires said user or Admin to be logged in)",
                        "Update User (requires said user or Admin to be logged in)",
                        "Delete User (requires said user or Admin to be logged in)"};
                        string exit = "Go back to previous menu";
                        var option = MenuTemplate(menuItems, title, exit);

                        switch (option)
                        {
                            case "1":
                                ShowUserCreateMenu();
                                break;
                            case "2":
                                ShowUserLoginMenu();
                                break;
                            case "3":
                                ShowUserLogoutMenu();
                                break;
                            case "4":
                                ShowActiveUserDetailsMenu();
                                break;
                            case "5":
                                ShowUserUpdateMenu();
                                break;
                            case "6":
                                ShowUserDeleteMenu();
                                break;
                            case "0":
                                userRegLoop = false;
                                break;
                            default:
                                break;
                        }

                        void ShowUserCreateMenu()
                        {
                            UserRegistrationDto user = new();
                            SubMenuTemplate("Fill in details of new user");

                            CWBreak("First Name: ");
                            user.FirstName = Console.ReadLine()!;
                            CW("Last Name: ");
                            user.LastName = Console.ReadLine()!;
                            CW("Phone number: ");
                            user.PhoneNumber = Console.ReadLine()!;
                            CW("Role: ");
                            user.UserRoleName = Console.ReadLine()!;
                            CW("Street name: ");
                            user.StreetName = Console.ReadLine()!;
                            CW("Postal code: ");
                            user.PostalCode = Console.ReadLine()!;
                            CW("City: ");
                            user.City = Console.ReadLine()!;
                            CW("Country: ");
                            user.Country = Console.ReadLine()!;
                            CW("Email: ");
                            user.Email = Console.ReadLine()!;
                            CW("Password: ");
                            user.Password = Console.ReadLine()!;

                            SubMenuTemplate("Create user Status");
                            PrintUserToBeCreated(user);
   
                            var customerAnswer = Question("Do you wish to create a User with the following information?");
                            if (AnswerIsYes(customerAnswer))
                            {
                                var result = _userRegistrationService.CreateNewUser(user);
                                SubMenuTemplate("Create user Status");
                                if (result.Item1 != null && result.Item2 != null)
                                {
                                    CWLBreak("User, Customer and Address created succesfully.\nMake a note of your password for future reference.");
                                    CWLBreak("Please Login to access your account");
                                }
                                else
                                    CWLBreak("Something went wrong, user not created.\nPlease make sure no fields are empty and try again.");
                            }
                            else
                                Console.WriteLine("\nUser was not created.");

                            PressKeyAndContinue();
                        }

                        void ShowUserLoginMenu()
                        {
                            UserDto user = new();

                            SubMenuTemplate("Login user");

                            CWBreak("\nEmail: ");
                            user.Email = Console.ReadLine()!;
                            CW("Password: ");
                            user.Password = Console.ReadLine()!;

                            SubMenuTemplate("Login status");
                            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                            {
                                CWLBreak("Error, user could not be logged in - Please make sure no fields are blank and try again");
                            }
                            else
                            {
                                var result = _userService.UserLogin(user);
                                SubMenuTemplate("Login status");
                                if (result)
                                {
                                    CWLBreak($"{user.Email} was succesfully logged in.");
                                }
                                else
                                    CWLBreak("Login attempt failed - password or email not recognized.");
                            }

                            PressKeyAndContinue();
                        }

                        void ShowUserLogoutMenu()
                        {
                            var result = _userService.LogOutActiveUser();
                            SubMenuTemplate("Logout status");
                            if (result)
                            {
                                CWLBreak("User was succesfully logged out.");
                            }
                            else
                                CWLBreak("Logout attempt failed - email not recognized / no users currently logged in.");

                            PressKeyAndContinue();
                        }

                        void ShowActiveUserDetailsMenu()
                        {
                            UserDto userDto = new();

                            SubMenuTemplate("Show User details");

                            CWBreak("Email: ");
                            userDto.Email = Console.ReadLine()!;

                            var existingUser = _userService.GetOne(userDto);
                            SubMenuTemplate("User details result:");
                            if (existingUser != null)
                            {
                                PrintUser(existingUser);
                            }
                            else
                                CWLBreak("No user found with that email.");

                            PressKeyAndContinue();
                        }

                        void ShowUserUpdateMenu()
                        {
                            UserDto existingUser = new();
                            UserDto newUserDetails = new();

                            SubMenuTemplate("Update user - fill in email of the user to update");

                            CWBreak("Email: ");
                            existingUser.Email = Console.ReadLine()!;

                            SubMenuTemplate($"Update {existingUser.Email} - fill in new user details");
                            CWLBreak("Any fields left empty will keep their current value");

                            CWBreak("Email: ");
                            newUserDetails.Email = Console.ReadLine()!;
                            CW("Password: ");
                            newUserDetails.Password = Console.ReadLine()!;

                            var existingUserRoles = _userRoleService.GetAll();
                            if (existingUserRoles.Count() >= 1)
                            {
                                foreach (var role in existingUserRoles)
                                {
                                    PrintRole(role);
                                }
                            }
                            CWLBreak("Please select the Id of the Role you wish to update to");
                            CWBreak("Role Id: ");
                            var idResult = int.TryParse(Console.ReadLine()!, out int id);
                            if (idResult)
                            {
                                newUserDetails.UserRoleId = id;
                            }

                            var updatedUser = _userService.UpdateUser(existingUser, newUserDetails);
                            SubMenuTemplate("Update status");
                            if (updatedUser != null)             
                                Console.WriteLine(
                                    $"\n{"",-5}User was updated to:" +
                                    $"\n{"",-5}Id:{"",-17}{updatedUser.Id}" +
                                    $"\n{"",-5}Email:{"",-14}{updatedUser.Email}" +
                                    $"\n{"",-5}If you made changes to your password please store it in a secure location.");
                            else
                                Console.WriteLine(
                                    $"\n{"",-5}{existingUser.Email} could not be updated!" +
                                    $"\n{"",-5}Please make sure {existingUser.Email} is logged in and is a valid email.");

                            PressKeyAndContinue();
                        }

                        void ShowUserDeleteMenu()
                        {
                            UserDto existingUser = new();

                            SubMenuTemplate("Delete user");

                            CWLBreak("Type in email of the user to delete.");
                            CWBreak("Email: ");
                            existingUser.Email = Console.ReadLine()!;

                            var userToDelete = _userService.GetOne(existingUser);
                            SubMenuTemplate("Delete Status");
                            if (userToDelete != null)
                            {
                                PrintUser(userToDelete);

                                var answer = Question("Is this the user you wish to delete?");
                                if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var deletedUser = _userService.DeleteUser(existingUser);
                                    SubMenuTemplate("Delete status");
                                    if (deletedUser != null)
                                    {
                                        PrintUser(deletedUser);
                                        CWLBreak("Was deleted.");
                                    }
                                    else
                                        Console.WriteLine(
                                            $"\n{"",-5}{existingUser.Email} could not be deleted!" +
                                            $"\n{"",-5}Please make sure {existingUser.Email} is logged in and is a valid email.");
                                }
                            }
                            else
                                Console.WriteLine(
                                    $"\n{"",-5}No user with email: {existingUser.Email} was found." +
                                    $"\n{"",-5}Please try again with a valid email.");

                            PressKeyAndContinue();
                        }
                    }
                }

                void ShowCustomerOptionsMenu()
                {
                    bool customerLoop = true;
                    while (customerLoop)
                    {
                        string title = "Customer menu";
                        string[] menuItems = {
                        "Show Customer details",
                        "Show all Customers",
                        "Update Customer (requires said user to be logged in)"};
                        string exit = "Go back to previous menu";
                        var option = MenuTemplate(menuItems, title, exit);

                        switch (option)
                        {
                            case "1":
                                ShowCustomerDetails();
                                break;
                            case "2":
                                ShowAllCustomers();
                                break;
                            case "3":
                                ShowUpdateCustomer();
                                break;
                            case "0":
                                customerLoop = false;
                                break;
                            default:
                                break;
                        }
                    }

                    void ShowCustomerDetails()
                    {
                        CustomerDto customer = new();

                        SubMenuTemplate("Get customer details");

                        CWBreak("Please enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        var customerDetails = _customerService.GetOneCustomerWithDetails(customer);
                        SubMenuTemplate("Customer details");
                        if (customerDetails.userRole != null && customerDetails.customer != null)
                        {
                            PrintCustomer(customerDetails.customer);
                            PrintRole(customerDetails.userRole);
                            CWLBreak($"List of addresses associated with {customerDetails.customer.FirstName} {customerDetails.customer.LastName}:");

                            if (customerDetails.address != null)
                            {   
                                foreach (var address in customerDetails.address)
                                {
                                    PrintAddress(address);  
                                }
                            }
                            else
                                CWBreak("There are currently no addresses linked to this customer");
                        }
                        else
                        {
                            SubMenuTemplate("Error - No customer details found!");
                            CWLBreak($"There are currently no customers associated with {customer.EmailId}");
                        }

                        PressKeyAndContinue();
                    }

                    void ShowAllCustomers()
                    {
                        var allCustomers = _customerService.GetAll();

                        SubMenuTemplate("All current customers");
                        if (allCustomers.Any())
                        {
                            foreach (var customer in allCustomers)
                            {
                                PrintCustomer(customer);
                            }
                        }
                        else
                            CWLBreak("There are currently no customers to display.");

                        PressKeyAndContinue();
                    }

                    void ShowUpdateCustomer()
                    {
                        CustomerDto newCustomerDetails = new();

                        SubMenuTemplate("Update customer");
                        CWLBreak("Fill in email of customer to update:");
                        CWBreak("Email: ");

                        newCustomerDetails.EmailId = Console.ReadLine()!;
                        var exisitingCustomer = _customerService.GetOneCustomer(newCustomerDetails);

                        SubMenuTemplate("Update status");
                        PrintCustomer(exisitingCustomer);

                        var customerAnswer = Question("Is this the customer you wish to update");
                        if (AnswerIsYes(customerAnswer))
                        {
                            SubMenuTemplate($"Update {exisitingCustomer.EmailId} - fill in new customer details.");
                            CWLBreak("Any fields left empty will keep their current value (Email is updated from 'Update User menu')");

                            CWBreak("First name: ");
                            newCustomerDetails.FirstName = Console.ReadLine()!;
                            CW("\nLast name: ");
                            newCustomerDetails.LastName = Console.ReadLine()!;
                            CW("\nPhone number: ");
                            newCustomerDetails.PhoneNumber = Console.ReadLine()!;

                            Console.WriteLine(
                                $"\n{"",-5}{exisitingCustomer.Id}" +
                                $"\n{"",-5}{exisitingCustomer.EmailId}" +
                                $"\n{"",-5}{(!string.IsNullOrWhiteSpace(newCustomerDetails.FirstName) ? newCustomerDetails.FirstName : exisitingCustomer.FirstName)}" +
                                $"\n{"",-5}{(!string.IsNullOrWhiteSpace(newCustomerDetails.LastName) ? newCustomerDetails.LastName : exisitingCustomer.LastName)}" +
                                $"\n{"",-5}{(!string.IsNullOrWhiteSpace(newCustomerDetails.PhoneNumber) ? newCustomerDetails.PhoneNumber : exisitingCustomer.PhoneNumber)}");
                            
                            var updateAnswer = Question("Do you want to update customer with the above details?");
                            if (AnswerIsYes(updateAnswer))
                            {
                                var customerResult = _customerService.UpdateCustomer(exisitingCustomer, newCustomerDetails);
                                SubMenuTemplate("Customer update status");
                                if (customerResult != null)
                                {
                                    PrintCustomer(customerResult);
                                    CWLBreak("Customer was updated succesfully.");
                                }
                                else
                                    CWLBreak("Customer failed to update, please try again - if issue persists contact support.");
                            }
                            else
                                CWLBreak("Customer was not updated");
                        }
                        else
                            CWLBreak("Customer will not be updated");

                        PressKeyAndContinue();

                    }

                }

                void ShowAddressOptionsMenu()
                {
                    bool addressloop = true;

                    while (addressloop)
                    {

                        string title = "Address menu";
                        string[] menuItems = {
                        "Create Address (requires user to be logged in)",
                        "Show Address details",
                        "Show all Addresses",
                        "Update Address (requires Admin privileges)",
                        "Delete Address (requires Admin privileges)"};
                        string exit = "Go back to previous menu";
                        var option = MenuTemplate(menuItems, title, exit);

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
                            case "4":
                                ShowUpdateAddress();
                                break;
                            case "5":
                                ShowDeleteAddress();
                                break;
                            case "0":
                                addressloop = false;
                                break;
                            default:
                                break;
                        }
                    }

                    void ShowCreateAddressMenu()
                    {
                        AddressDto address = new();

                        SubMenuTemplate("Type in details of new address");

                        CWBreak("\nStreet name: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("\nPostal code: ");
                        address.PostalCode = Console.ReadLine()!;
                        CW("\nCity: ");
                        address.City = Console.ReadLine()!;
                        CW("\nCountry: ");
                        address.Country = Console.ReadLine()!;

                        PrintAddress(address);

                        var answer = Question("Do you want to create this address?");
                        if (AnswerIsYes(answer))
                        {
                            var newAddress = _addressService.CreateAddress(address)!;
                            if (newAddress != null)
                            {
                                PrintAddress(newAddress);
                                CWLBreak("Address was created or already exists.");
                            }
                            else
                                CWLBreak("Address could not be created, please try again.");
                        }
                        else
                            CWLBreak("Address was not created.");

                        PressKeyAndContinue();
                    }

                    void ShowAddressDetails()
                    {
                        AddressDto address = new();

                        SubMenuTemplate("Get address details");
                        CWLBreak("Please enter Street name and postal code of Address:");

                        CWBreak("Street name: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("Postal Code: ");
                        address.PostalCode = Console.ReadLine()!;

                        var addressDetails = _addressService.GetOneAddressWithCustomers(address);
                        SubMenuTemplate("Address details");
                        if (addressDetails.address != null)
                        {
                            PrintAddress(addressDetails.address);
                            CWLBreak($"List of customers associated with {addressDetails.address.StreetName}, {addressDetails.address.PostalCode}:");

                            if (addressDetails.customers != null)
                            {
                                foreach (var customer in addressDetails.customers)
                                {
                                    PrintCustomer(customer);
                                }
                            }
                            else
                                CWLBreak("There are currently no customers linked to this Address");
                        }
                        else
                            CWLBreak($"There are currently no customers linked to {address.StreetName}, {address.PostalCode}");

                        PressKeyAndContinue();
                    }

                    void ShowAllAddresses()
                    {
                        var allAddresses = _addressService.GetAll();

                        SubMenuTemplate("All current addresses");

                        foreach (var address in allAddresses)
                        {
                            PrintAddress(address);
                        }

                        PressKeyAndContinue();
                    }

                    void ShowUpdateAddress()
                    {
                        AddressDto address = new();
                        AddressDto newAddressDetails = new();

                        SubMenuTemplate("Get address details");
                        CWLBreak("Please enter Street name and postal code of Address");

                        CWBreak("Street name: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("Postal Code: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingAddress = _addressService.GetOneAddress(address);
                        if (existingAddress != null)
                        {
                            SubMenuTemplate("Update status");
                            PrintAddress(existingAddress);

                            var answer = Question("Is this the address you wish to update?");
                            if (AnswerIsYes(answer))
                            {
                                SubMenuTemplate($"Update {existingAddress.StreetName}, {existingAddress.PostalCode} - fill in new address details.");
                                CWLBreak("Any fields left empty will keep their current value.");
                                CWBreak("Street name: ");
                                newAddressDetails.StreetName = Console.ReadLine()!;
                                CW("Postal code: ");
                                newAddressDetails.PostalCode = Console.ReadLine()!;
                                CW("City: ");
                                newAddressDetails.City = Console.ReadLine()!;
                                CW("Country: ");
                                newAddressDetails.Country = Console.ReadLine()!;

                                Console.WriteLine($"" +
                                    $"\n{"",-5}{(string.IsNullOrWhiteSpace(newAddressDetails.StreetName) ? existingAddress.StreetName : newAddressDetails.StreetName)}" +
                                    $"\n{"",-5}{(string.IsNullOrWhiteSpace(newAddressDetails.PostalCode) ? existingAddress.PostalCode : newAddressDetails.PostalCode)}" +
                                    $"\n{"",-5}{(string.IsNullOrWhiteSpace(newAddressDetails.City) ? existingAddress.City : newAddressDetails.City)}" +
                                    $"\n{"",-5}{(string.IsNullOrWhiteSpace(newAddressDetails.Country) ? existingAddress.Country : newAddressDetails.Country)}\n");

                                var updateAnswer = Question("Do you want to update address with the above details?");
                                if (AnswerIsYes(updateAnswer))
                                {
                                    var addressResult = _addressService.UpdateAddress(existingAddress, newAddressDetails);
                                    SubMenuTemplate("Update status");
                                    if (addressResult != null)
                                    {
                                        PrintAddress(addressResult);
                                        CWLBreak("Address succesfully updated");
                                    }
                                    else
                                        CWLBreak("Address failed to update, ensure you are logged in as Admin.");
                                }
                                else
                                    CWLBreak("Address will not be updated");
                            }   
                            else
                                CWLBreak("Address will not be updated");
                        }
                        else
                            Console.WriteLine("Address not found.\nMake sure all fields are filled in with valid details.");

                        PressKeyAndContinue();
                    }

                    void ShowDeleteAddress()
                    {
                        AddressDto address = new();

                        SubMenuTemplate("Delete Address");
                        CWLBreak("Fill in Street name and postal code of address to delete");

                        CWBreak("Street name: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("Postal Code: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingAddress = _addressService.GetOneAddress(address);
                        SubMenuTemplate("Delete status");
                        if (existingAddress != null)
                        {
                            PrintAddress(existingAddress);

                            var answer = Question("Is this the address you wish to delete? This action can not be undone.");
                            if (AnswerIsYes(answer))
                            {
                                var deletedAddress = _addressService.DeleteAddress(existingAddress);
                                SubMenuTemplate("Delete status");
                                if (deletedAddress != null)
                                {
                                    PrintAddress(deletedAddress);
                                }
                                else
                                    CWBreak("Address failed to delete, ensure you are logged in as Admin.");
                            }
                            else
                                CWBreak("Address will not be deleted.");
                        }
                        else
                            CWBreak("Address not found. Make sure all fields are filled in with valid details.");

                        PressKeyAndContinue();
                    }
                }

                void ShowCustomer_AddressOptions()
                {
                    bool Customer_AddressLoop = true;

                    while (Customer_AddressLoop)
                    {
                        string title = "Customer Address menu";
                        string[] menuItems = {
                        "Create Customer Address (requires user to be logged in)",
                        "Show Customer Address details",
                        "Show all Customer Addresses",                        
                        "Delete Customer Address (requires Admin privileges)"};
                        string exit = "Go back to previous menu";
                        var option = MenuTemplate(menuItems, title, exit);

                        switch (option)
                        {
                            case "1":
                                ShowCreateCustomer_AddressMenu();
                                break;
                            case "2":
                                ShowGetCustomer_AddressDetails();
                                break;
                            case "3":
                                ShowGetAllCustomer_AddressesMenu();
                                break;
                            case "4":
                                ShowDeleteCustomer_AddressMenu();
                                break;
                            case "0":
                                Customer_AddressLoop = false;
                                break;
                            default:
                                break;
                        }
                    }

                    void ShowCreateCustomer_AddressMenu()
                    {
                        SubMenuTemplate("Type in details of new customer_address");

                        CustomerDto customer = new();
                        AddressDto address = new();

                        CWBreak("Please enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        CWBreak("Please enter Street name of address: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("Please enter postal code of address: ");
                        address.PostalCode = Console.ReadLine()!;

                        var result = _customer_addressService.CreateCustomer_Address(customer, address);
                        SubMenuTemplate("Customer_Address status");
                        if (result)
                        {
                            CWBreak("Customer Address created.");
                        }
                        else
                            CWBreak("Customer Address could not be created or already exists.");

                        PressKeyAndContinue();
                    }

                    void ShowGetCustomer_AddressDetails()
                    {
                        SubMenuTemplate("Fill in details of customer_address to view details");

                        CustomerDto customer = new();
                        AddressDto address = new();

                        CWBreak("Please enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        CWBreak("Please enter Street name of address: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("Please enter postal code of address: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingCustomer_Address = _customer_addressService.GetCustomer_Address(customer, address);
                        SubMenuTemplate("Customer Address");
                        if (existingCustomer_Address != null)
                        {
                            PrintCustomer(CustomerFactory.Create(existingCustomer_Address.Customer));
                            PrintAddress(AddressFactory.Create(existingCustomer_Address.Address));      
                        }
                        else
                            CWLBreak("No Customer Address was found with those details.");

                        PressKeyAndContinue();
                    }

                    void ShowGetAllCustomer_AddressesMenu()
                    {
                        var existingCustomer_Addresses = _customer_addressService.GetAllCustomer_Addresses();
                        if (existingCustomer_Addresses != null)
                        {
                            SubMenuTemplate("All current Customer Addresses");
                            
                            foreach (Customer_AddressDto customer_Address in existingCustomer_Addresses)
                            {
                                string hyphens = new string('-', 56);
                                Console.WriteLine(
                                    $"\n{"",-5}{hyphens}" +
                                    $"\n{"",-5}Email:{"",-14}{customer_Address.Customer.EmailId}" +
                                    $"\n{"",-5}Customer Id:{"",-8}{customer_Address.CustomerId}" +
                                    $"\n" +
                                    $"\n{"",-5}Address Id:{"",-9}{customer_Address.AddressId}" +
                                    $"\n{"",-5}Street name:{"",-8}{customer_Address.Address.StreetName}" +
                                    $"\n{"",-5}Postal code:{"",-8}{customer_Address.Address.PostalCode}");
                            }
                        }
                        else
                            CWLBreak("There are currently no Customer Addresses.");

                        PressKeyAndContinue();
                    }

                    void ShowDeleteCustomer_AddressMenu()
                    {
                        SubMenuTemplate("Fill in details of customer_address to delete");

                        CustomerDto customer = new();
                        AddressDto address = new();

                        CWBreak("Please enter email of customer");
                        customer.EmailId = Console.ReadLine()!;

                        CW("Please enter Street name of address: ");
                        address.StreetName = Console.ReadLine()!;
                        CW("Please enter postal code of address: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingCustomer_Address = _customer_addressService.GetCustomer_Address(customer, address);
                        SubMenuTemplate("Customer Address");
                        if (existingCustomer_Address != null)
                        {
                            PrintCustomer(CustomerFactory.Create(existingCustomer_Address.Customer));
                            PrintAddress(AddressFactory.Create(existingCustomer_Address.Address));      

                            var answer = Question("Is this the Customer Address connection you wish to delete?");
                            SubMenuTemplate("Delete status");
                            if (AnswerIsYes(answer))
                            {
                                var deletedCustomer_Address = _customer_addressService.DeleteCustomer_Address(existingCustomer_Address);
                                SubMenuTemplate("Delete Status");
                                if (deletedCustomer_Address != null)
                                {
                                    PrintCustomer(CustomerFactory.Create(existingCustomer_Address.Customer));
                                    PrintAddress(AddressFactory.Create(existingCustomer_Address.Address));

                                    CWLBreak("Customer Address connecting these two entities has been deleted.");
                                }
                                else
                                    CWLBreak("Customer Address could not be deleted.");
                            }
                            else
                                CWLBreak("Customer Address will not be deleted.");
                        }
                        else
                            CWLBreak("No Customer Address was found with those details.");

                        PressKeyAndContinue();
                    }
                }
            }
        }

        void ShowProductMenu()
        {
            bool productLoop = true;

            while (productLoop)
            {
                string title = "Product menu";
                string[] menuItems = {
                    "Create Product (requires Admin privileges)",
                    "Show Product details", "Show all Product",
                    "Update Product (requires Admin privileges)",
                    "Delete Product (requires Admin privileges)",
                    "Price list Menu",
                    "Category Menu" };
                string exit = "Go back to previous menu";
                var option = MenuTemplate(menuItems, title, exit);

                switch (option)
                {
                    case "1":
                        ShowCreateProductMenu();
                        break;
                    case "2":
                        ShowProductDetailsMenu();
                        break;
                    case "3":
                        ShowAllProductsMenu();
                        break;
                    case "4":
                        ShowUpdateProductMenu();
                        break;
                    case "5":
                        ShowDeleteProductMenu();
                        break;
                    case "6":
                        ShowPriceListMenu();
                        break;
                    case "7":
                        ShowCategoryMenu();
                        break;
                    case "0":
                        productLoop = false;
                        break;
                    default:
                        break;
                }
            }

            void ShowCreateProductMenu()
            {
                var product = new ProductRegistrationDto();

                SubMenuTemplate("Create Product menu");

                Console.WriteLine("\nFill in details of new product. Required fields are marked with *\n");

                CW("\nArticle Number*: ");
                product.ArticleNumber = Console.ReadLine()!;

                CW("\nProduct Title*: ");
                product.Title = Console.ReadLine()!;

                CW("\nProduct Ingress: ");
                product.Ingress = Console.ReadLine()!;

                CW("\nProduct description: ");
                product.Description = Console.ReadLine()!;

                CW("\nCategory*: ");
                product.CategoryName = Console.ReadLine()!;

                CW("\nUnit (sold as: each, pair, set of X, etc)*: ");
                product.Unit = Console.ReadLine()!;

                CW("\nNumber of product items in stock*: ");
                var stockresult = int.TryParse(Console.ReadLine()!, out int stock);
                if (stockresult)
                {
                    product.Stock = stock;
                }
                else
                    product.Stock = 0;

                CW("\nPrice*: ");
                var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                if (priceResult)
                {
                    product.Price = price;
                }
                else
                    product.Price = 0;

                CW("\nCurrency (ie. SEK, USD, EUR)*: ");
                product.Currency = Console.ReadLine()!;

                CW("\nDiscount price: ");
                var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                if (discountPriceResult)
                {
                    product.DiscountPrice = discountPrice;
                }
                else
                    product.DiscountPrice = 0;

                var newProduct = _productService.CreateProduct(product);
                if (newProduct != null)
                {
                    var productDisplay = _productService.GetProductDisplay(newProduct);
                    if (productDisplay != null)
                    {
                        SubMenuTemplate("New Product created");
                        PrintProductDetails(productDisplay);
                    }
                    else
                        CWLBreak("Product could not be created");

                    PressKeyAndContinue();
                }
            }

            void ShowProductDetailsMenu()
            {
                ProductDto productDto = new();
                SubMenuTemplate("Show product details");
                CWBreak("Fill in article number of product to show: ");
                productDto.ArticleNumber = Console.ReadLine()!;

                var product = _productService.GetProductDisplay(productDto);
                SubMenuTemplate("Product search results:");
                if (product != null)
                {
                    PrintProductDetails(product);
                }
                else
                    CWLBreak("No product was found with that article number.");

                PressKeyAndContinue();
            }

            void ShowAllProductsMenu()
            {
                var products = _productService.GetAllProducts();
                SubMenuTemplate("All Products");
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        PrintProductDetails(product);
                    }
                }
                else
                    CWLBreak("No products to show currently.");

                PressKeyAndContinue();
            }

            void ShowUpdateProductMenu()
            {
                ProductDto productDto = new();
                SubMenuTemplate("Update product details");
                CWBreak("Fill in article number of product to update: ");
                productDto.ArticleNumber = Console.ReadLine()!;

                var product = _productService.GetProductDisplay(productDto);
                SubMenuTemplate("Product search:");
                if (product != null)
                {
                    PrintProductDetails(product);  

                    var answer = Question("Is this the product to update?");
                    if (AnswerIsYes(answer))
                    {
                        ProductRegistrationDto updatedProductDetails = new();

                        SubMenuTemplate("New product details:");
                        CWLBreak("Fill in updated details of product.´Fields left empty will retain their current value.");
                        CWL("Please note that article number can not be changed, instead create a new product with a different article number.");

                        updatedProductDetails.ArticleNumber = product.ArticleNumber;

                        CW("\nProduct Title: ");
                        updatedProductDetails.Title = Console.ReadLine()!;
                        CW("\nProduct Ingress: ");
                        updatedProductDetails.Ingress = Console.ReadLine()!;
                        CW("\nProduct description: ");
                        updatedProductDetails.Description = Console.ReadLine()!;
                        CW("\nCategory: ");
                        updatedProductDetails.CategoryName = Console.ReadLine()!;
                        CW("\nUnit (sold as: each, pair, set of X, etc): ");
                        updatedProductDetails.Unit = Console.ReadLine()!;

                        CW("\nAdjust the number of product items in stock (prefix with - to reduce amount): ");
                        var stockresult = int.TryParse(Console.ReadLine()!, out int stock);
                        if (stockresult)
                        {
                            updatedProductDetails.Stock = stock;
                        }
                        else
                        {
                            updatedProductDetails.Stock = product.Stock;
                            CWL("Stock was not updated.");
                        }

                        CW("\nPrice: ");
                        var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                        if (priceResult)
                        {
                            updatedProductDetails.Price = price;
                        }
                        else
                        {
                            updatedProductDetails.Price = product.Price;
                            CWL("Price was not updated.");
                        }

                        CW("\nCurrency (ie. SEK, USD, EUR): ");
                        product.Currency = Console.ReadLine()!;

                        CW("\nDiscount price: ");
                        var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                        if (discountPriceResult)
                        {
                            updatedProductDetails.DiscountPrice = discountPrice;
                        }
                        else
                        {
                            updatedProductDetails.DiscountPrice = product.DiscountPrice;
                            CWL("Discount price was not updated.");
                        }

                        var updatedProduct = _productService.UpdateProduct(updatedProductDetails);
                        SubMenuTemplate("Product update status");
                        if (updatedProduct != null)
                        {
                            var productDisplay = _productService.GetProductDisplay(updatedProduct);
                            if (productDisplay != null)
                            {
                                SubMenuTemplate("Product updated");
                                PrintProductDetails(productDisplay);
                                CWLBreak("Product updated succesfully.");
                            }
                        }
                        else
                            CWLBreak("Product could not be updated.");
                    }
                    else
                        CWLBreak("Product will not be updated.");
                }
                else
                    CWLBreak("No product found with that article number.");

                PressKeyAndContinue();
            }

            void ShowDeleteProductMenu()
            {
                ProductDto productDto = new();
                SubMenuTemplate("Delete product");
                CWBreak("Fill in article number of product to delete: ");
                productDto.ArticleNumber = Console.ReadLine()!;

                var product = _productService.GetProductDisplay(productDto);
                SubMenuTemplate("Product search:");
                if (product != null)
                {
                    PrintProductDetails(product);

                    var answer = Question("Is this the product to delete?");
                    if (AnswerIsYes(answer))
                    {
                        var deletedProduct = _productService.DeleteProduct(productDto);
                        SubMenuTemplate("Delete Status:");
                        if (deletedProduct != null)
                        {
                            Console.WriteLine(
                                $"\n{"",-5}Article number:{"",-5}{deletedProduct.ArticleNumber}" +
                                $"\n{"",-5}Title:{"",-14}{deletedProduct.Title}" +
                                $"\n{"",-5}Ingress:{"",-12}{deletedProduct.Ingress}" +
                                $"\n{"",-5}Category:{"",-11}{deletedProduct.Category.CategoryName}");

                            CWLBreak("Was deleted.");
                        }
                        else
                            CWLBreak("Product could not be deleted.");
                    }
                    else
                        CWLBreak("Product will not be deleted.");
                }
                else
                    CWLBreak("There was no product found with that article number.");

                PressKeyAndContinue();
            }

            void ShowPriceListMenu()
            {
                bool priceListLoop = true;

                while (priceListLoop)
                {
                    string title = "PriceList menu";
                    string[] menuItems ={
                    "Create Price list (requires Admin privileges)",
                    "Show Price list details",
                    "Show all Price list",
                    "Update Price list (requires Admin privileges)",
                    "Delete Price list (requires Admin privileges)" };
                    string exit = "Go back to previous menu";
                    var option = MenuTemplate(menuItems, title, exit);

                    switch (option)
                    {
                        case "1":
                            ShowCreatePriceListMenu();
                            break;
                        case "2":
                            ShowPriceListDetailsMenu();
                            break;
                        case "3":
                            ShowAllPriceListsMenu();
                            break;
                        case "4":
                            ShowUpdatePriceListMenu();
                            break;
                        case "5":
                            ShowDeletePriceListMenu();
                            break;
                        case "0":
                            priceListLoop = false;
                            break;
                        default:
                            break;
                    }

                }

                void ShowCreatePriceListMenu()
                {
                    PriceListDto priceListDto = new();

                    SubMenuTemplate("Create Price list");
                    CWLBreak("Fill in details for new price list. Required fields are marked with *");

                    CW("\nPrice *: ");
                    var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                    if (priceResult)
                    {
                        priceListDto.Price = price;
                    }

                    CW("\nDiscount price: ");
                    var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                    if (discountPriceResult)
                    {
                        priceListDto.DiscountPrice = discountPrice;
                    }

                    CW("\nCurrency (ie. SEK, EUR, USD) *: ");
                    priceListDto.UnitType = Console.ReadLine()!;


                    SubMenuTemplate("Price list status");
                    PrintPriceList(priceListDto);

                    var answer = Question("Is this the price list you want to create?");
                    if (AnswerIsYes(answer))
                    {
                        var newPriceList = _priceListService.CreatePriceList(priceListDto);
                        SubMenuTemplate("Price list status");
                        if (newPriceList != null)
                        {
                            CWLBreak("Price list created:");
                            PrintPriceList(newPriceList);
                        }
                        else
                            CWLBreak("Price list could not be created, make sure you are logged in as Admin.");
                    }
                    else
                        CWLBreak("Price list was not created.");

                    PressKeyAndContinue();
                }

                void ShowPriceListDetailsMenu()
                {
                    PriceListDto priceListDto = new();

                    var allPriceLists = _priceListService.GetAllPriceLists();
                    SubMenuTemplate("All current Price lists:");
                    if (allPriceLists != null)                    {

                        foreach (var priceList in allPriceLists)
                        {
                            PrintPriceList(priceList);
                        }

                        CWLBreak("Fill in Id of Price list to show details");

                        CWBreak("Id: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            priceListDto.Id = Id;
                        }

                        var priceListDetails = _priceListService.GetPriceList(priceListDto);
                        SubMenuTemplate("Price list details");
                        if (priceListDetails != null)
                        {
                            CWLBreak("Price list:");
                            PrintPriceList(priceListDetails);

                            CWLBreak("Products currently associated to this Price list:");

                            foreach (var product in priceListDetails.Products)
                            {
                                PrintProduct(product);
                            }
                        }
                        else
                            CWLBreak("No Price Lists with that Id.");
                    }
                    else
                        CWLBreak("No current Price Lists to show.");

                    PressKeyAndContinue();
                }

                void ShowAllPriceListsMenu()
                {
                    var priceLists = _priceListService.GetAllPriceLists();
                    SubMenuTemplate("All Price lists");
                    if (priceLists != null)
                    {
                        foreach (var priceList in priceLists)
                        {
                            PrintPriceList(priceList);
                        }
                    }
                    else
                        CWLBreak("There are currently no Price lists.");

                    PressKeyAndContinue();
                }

                void ShowUpdatePriceListMenu()
                {
                    PriceListDto priceListDto = new();

                    var allPriceLists = _priceListService.GetAllPriceLists();
                    SubMenuTemplate("All current Price lists:");
                    if (allPriceLists != null)
                    {
                        foreach (var priceList in allPriceLists)
                        {
                            PrintPriceList(priceList);
                        }

                        CWLBreak("Fill in Id of Price list to update");

                        CWBreak("Id: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            priceListDto.Id = Id;
                        }

                        var existingPriceList = _priceListService.GetPriceList(priceListDto);
                        SubMenuTemplate("Price list to update");
                        if (existingPriceList != null)
                        {
                            PrintPriceList(existingPriceList);

                            CWLBreak("Products that will be affected by this update:");

                            foreach (var product in existingPriceList.Products)
                            {
                                PrintProduct(product);
                            }

                            var answer = Question("Do you want to update this pricelist?");
                            SubMenuTemplate("Update Price list details");
                            if (AnswerIsYes(answer))
                            {
                                PriceListDto updatedPricelistDetails = new();

                                CWL("Fill in updated details for price list. Any fields left empty will keep their original values.");

                                updatedPricelistDetails.Id = existingPriceList.Id;

                                CWBreak("Price: ");
                                var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                                if (priceResult)
                                {
                                    updatedPricelistDetails.Price = price;
                                }

                                CW("Discount price: ");
                                var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                                if (discountPriceResult)
                                {
                                    updatedPricelistDetails.DiscountPrice = discountPrice;
                                }

                                CW("Currency (ie. SEK, EUR, USD): ");
                                updatedPricelistDetails.UnitType = Console.ReadLine()!;

                                SubMenuTemplate("Confirm update");  
                                PrintPriceList(updatedPricelistDetails);

                                var updateAnswer = Question("Do you wish to update Price list with these details?");
                                if (AnswerIsYes(updateAnswer))
                                {
                                    var updatedPriceList = _priceListService.UpdatePriceList(existingPriceList, updatedPricelistDetails);
                                    SubMenuTemplate("Update status");
                                    if (updatedPriceList != null)
                                    {
                                        PrintPriceList(updatedPriceList);
                                        CWLBreak("Price list succesfully updated.");
                                    }
                                    else
                                        CWLBreak("Price list could not be updated, make sure you are logged in as Admin.");
                                }
                                else
                                    CWLBreak("Price list was not updated.");
                            }
                            else
                                CWLBreak("Price List will not be updated.");
                        }
                        else
                            CWLBreak("No Price Lists with that Id.");
                    }
                    else
                        CWLBreak("No current Price Lists to show.");

                    PressKeyAndContinue();
                }

                void ShowDeletePriceListMenu()
                {
                    PriceListDto priceListDto = new();

                    var allPriceLists = _priceListService.GetAllPriceLists();
                    SubMenuTemplate("All current Price lists:");
                    if (allPriceLists != null)
                    {
                        foreach (var priceList in allPriceLists)
                        {
                            PrintPriceList(priceList);
                        }

                        CWLBreak("Fill in Id of Price list to delete");

                        CWBreak("Id: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            priceListDto.Id = Id;
                        }

                        var existingPriceList = _priceListService.GetPriceList(priceListDto);
                        SubMenuTemplate("Price list to delete");
                        if (existingPriceList != null)
                        {
                            PrintPriceList(existingPriceList);
                            CWLBreak("Products that will be affected by this update:");

                            foreach (var product in existingPriceList.Products)
                            {
                                PrintProduct(product);
                            }

                            var answer = Question("Do you want to delete this pricelist?");
                            if (AnswerIsYes(answer))
                            {
                                var deletedPriceList = _priceListService.DeletePriceList(existingPriceList);
                                SubMenuTemplate("Delete status");
                                if (deletedPriceList != null)
                                {
                                    PrintPriceList(deletedPriceList);
                                    CWLBreak("Price list succesfully Deleted.");
                                }
                                else
                                    CWLBreak("Price list could not be deleted, make sure you are logged in as Admin.");
                            }
                            else
                                CWLBreak("Price will not be deleted.");
                        }
                        else
                            CWLBreak("No Price Lists with that Id.");
                    }
                    else
                        CWLBreak("No current Price Lists to show.");

                    PressKeyAndContinue();
                }

            }

            void ShowCategoryMenu()
            {
                bool CategoryLoop = true;

                while (CategoryLoop)
                {
                    string title = "Category menu";
                    string[] menuItems ={
                    "Create Category (requires Admin privileges)",
                    "Show Category details",
                    "Show all Categories",
                    "Update Category (requires Admin privileges)",
                    "Delete Category (requires Admin privileges)" };
                    string exit = "Go back to previous menu";
                    var option = MenuTemplate(menuItems, title, exit);

                    switch (option)
                    {
                        case "1":
                            ShowCreateCategoryMenu();
                            break;
                        case "2":
                            ShowCategoryDetailsMenu();
                            break;
                        case "3":
                            ShowAllCategoriesMenu();
                            break;
                        case "4":
                            ShowUpdateCategoryMenu();
                            break;
                        case "5":
                            ShowDeleteCategoryMenu();
                            break;
                        case "0":
                            CategoryLoop = false;
                            break;
                        default:
                            break;
                    }

                }

                void ShowCreateCategoryMenu()
                {
                    CategoryDto categoryDto = new();

                    SubMenuTemplate("Create Category");
                    CWLBreak("Fill name for new Category. Id will be created automatically");

                    CWBreak("Category name: ");
                    categoryDto.CategoryName = Console.ReadLine()!;

                    SubMenuTemplate("Category status");
                    PrintCategory(categoryDto);

                    var answer = Question("Is this the Category you want to create?");
                    if (AnswerIsYes(answer))
                    {
                        var newCategory = _categoryService.CreateCategory(categoryDto);
                        SubMenuTemplate("Category status");
                        if (newCategory != null)
                        {
                            PrintCategory(newCategory);
                        }
                        else
                            CWLBreak("Category could not be created, make sure you are logged in as Admin.");
                    }
                    else
                        CWLBreak("Category was not created.");

                    PressKeyAndContinue();
                }

                void ShowCategoryDetailsMenu()
                {
                    CategoryDto categoryDto = new();

                    var categories = _categoryService.GetAllCategories();
                    SubMenuTemplate("All current Categories:");
                    if (categories != null)
                    {
                        foreach (var category in categories)
                        {
                            PrintCategory(category);
                        }

                        CWLBreak("Fill in Id of Category to show details");

                        CWBreak("Id: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            categoryDto.Id = Id;
                        }

                        var categoryDetails = _categoryService.GetCategory(categoryDto);
                        SubMenuTemplate("Category details");
                        if (categoryDetails != null)
                        {
                            PrintCategory(categoryDetails);

                            CWLBreak("Products currently associated to this Category:");

                            foreach (var product in categoryDetails.Products)
                            {
                                PrintProduct(product);
                            }
                        }
                        else
                            CWLBreak("No Category with that Id.");
                    }
                    else
                        CWLBreak("No current Categories to show.");

                    PressKeyAndContinue();
                }

                void ShowAllCategoriesMenu()
                {
                    var categories = _categoryService.GetAllCategories();
                    SubMenuTemplate("All current Categories:");
                    if (categories != null)
                    {
                        foreach (var category in categories)
                        {
                            PrintCategory(category);
                        }
                    }
                    else
                        CWLBreak("There are currently no Categories.");

                    PressKeyAndContinue();
                }

                void ShowUpdateCategoryMenu()
                {
                    CategoryDto categoryDto = new();

                    var categories = _categoryService.GetAllCategories();
                    SubMenuTemplate("All current Categories:");
                    if (categories != null)
                    {
                        foreach (var category in categories)
                        {
                            PrintCategory(category);
                        }

                        CWLBreak("Fill in Id of Category to update");

                        CWBreak("Id: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            categoryDto.Id = Id;
                        }

                        var existingCategory = _categoryService.GetCategory(categoryDto);
                        SubMenuTemplate("Category to update");
                        if (existingCategory != null)
                        {
                            PrintCategory(existingCategory);

                            CWLBreak("Products that will be affected by this update:");

                            foreach (var product in existingCategory.Products)
                            {
                                PrintProduct(product);
                            }

                            var answer = Question("\nDo you want to update this Category?");
                            SubMenuTemplate("Update Category details");
                            if (AnswerIsYes(answer))
                            {
                                CategoryDto updatedCategoryDetails = new();
                                updatedCategoryDetails.Id = existingCategory.Id;

                                CWLBreak("Fill in updated details for Category. Any fields left empty will keep their original values.");

                                CWBreak("Category name: ");
                                updatedCategoryDetails.CategoryName = Console.ReadLine()!;

                                SubMenuTemplate("Confirm update");
                                PrintCategory(updatedCategoryDetails);

                                var updateAnswer = Question("Do you wish to update Category with these details?");
                                if (AnswerIsYes(updateAnswer))
                                {
                                    var updatedCategory = _categoryService.UpdateCategory(existingCategory, updatedCategoryDetails);
                                    SubMenuTemplate("Update status");
                                    if (updatedCategory != null)
                                    {
                                        PrintCategory(updatedCategory);
                                        CWLBreak("Category succesfully updated.");
                                    }
                                    else
                                        CWLBreak("Category could not be updated, make sure you are logged in as Admin.");
                                }
                                else
                                    CWLBreak("Category was not updated.");
                            }
                            else
                                CWLBreak("Category will not be updated.");
                        }
                        else
                            CWLBreak("No Categories with that Id.");
                    }
                    else
                        CWLBreak("There are currently no Categories.");

                    PressKeyAndContinue();
                }

                void ShowDeleteCategoryMenu()
                {
                    CategoryDto categoryDto = new();

                    var allCategories = _categoryService.GetAllCategories();
                    SubMenuTemplate("All current Categories:");
                    if (allCategories != null)
                    {
                        foreach (var category in allCategories)
                        {
                            PrintCategory(category);
                        }

                        CWLBreak("Fill in Id of Category to delete");

                        CWBreak("Id: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            categoryDto.Id = Id;
                        }

                        var existingCategory = _categoryService.GetCategory(categoryDto);
                        SubMenuTemplate("Category to delete");
                        if (existingCategory != null)
                        {
                            PrintCategory(existingCategory);

                            CWLBreak("Products that will be affected by this update:");

                            foreach (var product in existingCategory.Products)
                            {
                                PrintProduct(product);
                            }

                            var answer = Question("\nDo you want to delete this Category?");
                            SubMenuTemplate("Delete Category details");
                            if (AnswerIsYes(answer))
                            {
                                var deletedCategory = _categoryService.DeleteCategory(existingCategory);
                                SubMenuTemplate("Update status");
                                if (deletedCategory != null)
                                {
                                    PrintCategory(deletedCategory);
                                    CWLBreak("Category succesfully deleted.");
                                }
                                else
                                    CWLBreak("Category could not be deleted, make sure you are logged in as Admin.");
                            }
                            else
                                CWLBreak("Category will not be deleted.");
                        }
                        else
                            CWLBreak("NoCategory with that Id.");
                    }
                    else
                        CWLBreak("No current Categories to show.");

                    PressKeyAndContinue();
                }
            }
        }

//------HELPER METHODS-------------------------------------------------------

        string MenuTemplate(string[] menuItems, string title, string exit)
        {
            int i = 1;
            Console.Clear();
            Console.WriteLine($"\n{"",-5}{title} - Choose an option");
            string hyphens = new string('-', $"{"",-5}{title} - Choose an option{"",-5}".Length);
            Console.WriteLine(hyphens);

            foreach (string item in menuItems)
            {
                Console.WriteLine($"\n{"",-5}{i++}.{"",-3}{item}.");
            }

            Console.WriteLine($"\n{"",-5}{"0.",-5}{exit}.");
            CW($"\n\n{"",-5}Option: ");

            var option = Console.ReadLine()!;
            return option;
        }

        string Question(string questionTextHere)
        {
            Console.WriteLine($"\n{"", -5}{questionTextHere}\n");
            Console.Write($"{"",-5}[Y]es / [N]o: ");
            var answer = Console.ReadLine()!;

            return answer;
        }

        bool AnswerIsYes(string answer)
        {
            var result = answer.Equals("y", StringComparison.CurrentCultureIgnoreCase);
            return result;
        }

        void SubMenuTemplate(string menuName)
        {
            Console.Clear();
            Console.WriteLine($"\n{"",-5}{menuName}");
            string hyphens = new string('-', $"{"",5}{menuName}{"",10}".Length);
            Console.WriteLine($"{"",-5}{hyphens}");
        }
        
        void PressKeyAndContinue()        
        {
            Console.Write($"\n{"",-5}Press any key to continue.");
            Console.ReadKey();
        }
        
        void PrintAddress(AddressDto address)
        {
            Console.WriteLine(
                $"\n{"",-5}Address Id:{"",-9}{address.Id}" +
                $"\n{"",-5}Street name:{"",-8}{address.StreetName}" +
                $"\n{"",-5}Postal code:{"",-8}{address.PostalCode}" +
                $"\n{"",-5}City:{"",-15}{address.City}" +
                $"\n{"",-5}Country:{"",-12}{address.Country}");
        }

        void PrintUserToBeCreated(UserRegistrationDto user)
        {
            Console.WriteLine(
                $"\n{"",-5}User:" +
                $"\n{"",-5}-----" +
                $"\n{"",-5}First name:{"",-9}{user.FirstName}" +
                $"\n{"",-5}Last name:{"",-10}{user.LastName}" +
                $"\n{"",-5}Email:{"",-14}{user.Email}" +
                $"\n{"",-5}Phone number:{"",-7}{user.PhoneNumber}" +
                $"\n{"",-5}Role:{"",-15}{user.UserRoleName}" +
                $"\n\n{"",-5}Address:" +
                $"\n{"",-5}--------" +
                $"\n{"",-5}Street name:{"",-8}{user.StreetName}" +
                $"\n{"",-5}Postal Code:{"",-8}{user.PostalCode}" +
                $"\n{"",-5}City:{"",-15}{user.City}" +
                $"\n{"",-5}Country:{"",-12}{user.Country}");
        }

        void PrintUser(UserDto user)
        {
            Console.WriteLine(
                $"\n{"",-5}User Id:{"",-12}{user.Id}" +
                $"\n{"",-5}Email:{"",-14}{user.Email}" +
                $"\n{"",-5}Logged in:{"",-10}{user.IsActive}" +
                $"\n{"",-5}Account created:{"",-4}{user.Created}");

        }

        void PrintRole(UserRoleDto role)
        {
            Console.WriteLine(
                $"\n{"",-5}Role Id:{"",-12}{role.Id}" +
                $"\n{"",-5}Role name:{"",-10}{role.RoleName}");
        }

        void PrintCustomer(CustomerDto customer)
        {
            Console.WriteLine(
                $"\n{"",-5}Customer:" +
                $"\n{"",-5}---------" +
                $"\n{"",-5}Customer Id:{"",-8}{customer.Id}" +
                $"\n{"",-5}First name:{"",-9}{customer.FirstName}" +
                $"\n{"",-5}Last name:{"",-10}{customer.LastName}" +
                $"\n{"",-5}Email:{"",-14}{customer.EmailId}" +
                $"\n{"",-5}Phone number:{"",-7}{customer.PhoneNumber}");
        }

        void PrintOrder(OrderDto order)
        {
            Console.WriteLine(
                $"\n{"",-5}Order:" +
                $"\n{"",-5}------" +
                $"\n{"",-5}Order Id:{"",-11}{order.Id}" +
                $"\n{"",-5}Customer Id:{"",-8}{order.CustomerId}" +
                $"\n{"",-5}Order Created:{"",-6}{order.OrderDate}" +
                $"\n{"",-5}Total Price:{"",-8}{order.OrderPrice}");
        }

        void PrintOrderRowLoop(OrderRowDto orderRow, ProductRegistrationDto product, decimal? price)
        {
            Console.WriteLine($"{"",-5}| {orderRow.ArticleNumber} | {product.Title} | {price} | {orderRow.Quantity} | {orderRow.OrderRowPrice} | {product.Currency} |");
            string hyphens = new string('-', $"| {orderRow.ArticleNumber} | {product.Title}| {price} | {orderRow.Quantity} | {orderRow.OrderRowPrice} | {product.Currency} |\n".Length);
            Console.WriteLine($"{"",-5}{hyphens}");
        }

        void PrintOrderRow(OrderRowDto orderRow)
        {
            Console.WriteLine(
              $"\n{"",-5}Order row Id:{"",-7}{orderRow.Id}" +
              $"\n{"",-5}Article number:{"",-5}{orderRow.ArticleNumber}" +
              $"\n{"",-5}Order Id:{"",-11}{orderRow.OrderId}" +
              $"\n{"",-5}Quantity:{"",-11}{orderRow.Quantity}" +
              $"\n{"",-5}Order row price:{"",-4}{orderRow.OrderRowPrice}");
        }

        void PrintProductDetails(ProductRegistrationDto product)
        {
            Console.WriteLine(
                $"\n{"",-5}Article number:{"",-5}{product.ArticleNumber}" +
                $"\n{"",-5}Title:{"",-14}{product.Title}" +
                $"\n{"",-5}Ingress:{"",-12}{product.Ingress}" +
                $"\n{"",-5}Description:{"",-8}{product.Description}" +
                $"\n{"",-5}Category:{"",-11}{product.CategoryName}" +
                $"\n{"",-5}Price:{"",-14}{product.Price} {product.Currency}" +
                $"\n{"",-5}Discount price:{"",-5}{product.DiscountPrice} {product.Currency}" +
                $"\n{"",-5}Unit:{"",-15}{product.Unit}" +
                $"\n{"",-5}Stock:{"",-14}{product.Stock}");
        }

        void PrintProduct(ProductEntity product)
        {
            Console.WriteLine(
               $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}" +
               $"\n{"",-5}Title: {"",-13}{product.Title}" +
               $"\n{"",-5}Category: {"",-10}{product.Category.CategoryName}" +
               $"\n{"",-5}Unit: {"",-14}{product.Unit}" +
               $"\n{"",-5}Stock: {"",-13}{product.Stock}");
        }

        void PrintPriceList(PriceListDto priceList)
        {
            Console.WriteLine(
                $"\n{"",-5}Price list Id:{"",-6}{priceList.Id}" +
                $"\n{"",-5}Price:{"",-14}{priceList.Price}" +
                $"\n{"",-5}Discount price:{"",-5}{priceList.DiscountPrice}" +
                $"\n{"",-5}Currency:{"",-11}{priceList.UnitType}");
        }

        void PrintCategory(CategoryDto category)
        {
            Console.WriteLine(
                $"\n{"",-5}Id:{"",-17}{category.Id}" +
                $"\n{"",-5}Category name:{"",-6}{category.CategoryName}");
        }

        void CWL(string text)
        {
            Console.WriteLine($"{"",-5}{text}");
        }

        void CWLBreak(string text)
        {
            Console.WriteLine($"\n{"",-5}{text}");
        }

        void CW(string text)
        {
            Console.Write($"{"",-5}{text}");
        }

        void CWBreak(string text)
        {
            Console.Write($"\n{"",-5}{text}");
        }
    }

}