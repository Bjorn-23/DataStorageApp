using Business.Dtos;
using Business.Services;

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
            Console.Clear();
            Console.WriteLine($"{"",-5}Björn's super Store - Choose an option");
            string hyphens = new string('-', $"{"",-5}Björn's super Store - Choose an option{"",-5}".Length);
            Console.WriteLine(hyphens);
            Console.WriteLine($"{"\n1.",-5} Login User");
            Console.WriteLine($"{"\n2.",-5} Create User");
            Console.WriteLine($"{"\n3.",-5} Webshop");
            Console.WriteLine($"{"\n4.",-5} User Settings Menu");
            Console.WriteLine($"{"\n5.",-5} Product Menu");
            Console.WriteLine($"{"\n0.",-5} Exit application and Logout User");
            Console.Write($"\n\n{"",-5}Option: ");
            var option = Console.ReadLine();

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

            Console.Write("\nEmail: ");
            user.Email = Console.ReadLine()!;

            Console.Write("Password: ");
            user.Password = Console.ReadLine()!;
            SubMenuTemplate("Login status");
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                Console.WriteLine("\nError, user could not be logged in - Please make sure no fields are blank and try again");
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

        void ShowUserCreateMenu()
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
            Console.WriteLine($"\n{"",-14}{user.StreetName}\n{"",-14}{user.PostalCode}\n{"",-14}{user.City}\n{"",-14}{user.Country}");

            Console.Write("\n[Y]es / [N]o: ");
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

        void ShowWebShopMenu()
        {
            bool webShopLoop = true;

            while (webShopLoop)
            {
                Console.Clear();
                Console.WriteLine($"{"",-5}Webshop menu - Choose an option");
                string hyphens = new string('-', $"{"",5}Webshop menu - Choose an option".Length);
                Console.WriteLine(hyphens);
                Console.WriteLine($"{"\n1.",-5} Show Order details");
                Console.WriteLine($"{"\n2.",-5} Create Order");
                Console.WriteLine($"{"\n3.",-5} Delete Order");
                Console.WriteLine($"{"\n4.",-5} Add product to Order");
                Console.WriteLine($"{"\n5.",-5} Show Order rows");
                Console.WriteLine($"{"\n6.",-5} Update Order rows");
                Console.WriteLine($"{"\n7.",-5} Delete Order rows");
                Console.WriteLine($"{"\n0.",-5} Go back");
                Console.Write($"\n\n{"",-5}Option: ");
                var option = Console.ReadLine();

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
                        ShowCreateOrderRowMenu();
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
                    Console.WriteLine("\nCustomer:\n");
                    Console.WriteLine($"Id: {"",-8}{result.customer.Id}\nName: {"",-6}{result.customer.FirstName} {result.customer.LastName}\nEmail: {"",-5}{result.customer.EmailId}\nPhone no: {"",-2}{result.customer.PhoneNumber}\n\n");

                    Console.WriteLine("Order:\n");
                    Console.WriteLine(
                        $"Order Id: {result.order.Id}\n" +
                        $"Order Created: {result.order.OrderDate}\n" +
                        $"Total Price: {result.order.OrderPrice}\n\n");

                    Console.WriteLine("Order rows:\n");

                    foreach (var orderRow in result.orderRows)
                    {
                        foreach (var product in result.products.Where(x => x.ArticleNumber == orderRow.ArticleNumber))
                        {
                            var price = product.DiscountPrice != null ? product.DiscountPrice : product.Price;

                            Console.WriteLine(
                                $"| {orderRow.ArticleNumber} | {product.Title}| {price} | {orderRow.Quantity} | {orderRow.OrderRowPrice} | {product.Currency} |");
                            string hyphens = new string('-', $"| {orderRow.ArticleNumber} | {product.Title}| {price} | {orderRow.Quantity} | {orderRow.OrderRowPrice} | {product.Currency} |\n".Length);
                            Console.WriteLine(hyphens);
                        }
                    }
                }
                else
                    Console.WriteLine("No orders associated with logged in user.");

                PressKeyAndContinue();
            }

            void ShowCreateOrderMenu()
            {
                var result = _orderService.CreateOrder();
                SubMenuTemplate("Created Order details");
                if (result != null)
                {
                    Console.WriteLine($"Id: {result.Id}\nCustomer Id: {result.CustomerId}\nOrder date: {result.OrderDate}\nOrder price: {result.OrderPrice}\n");
                }
                else
                    Console.WriteLine("Order already created, or no user logged in.");

                PressKeyAndContinue();
            }

            void ShowDeleteOrderMenu()
            {
                var order = _orderService.GetUsersOrder();
                SubMenuTemplate("Current Order details");
                if (order != null)
                {
                    Console.WriteLine("\nBe advised that any Order Rows associated with the order will also be deleted!");
                    Console.WriteLine($"\nId: {order.Id}\nCustomer Id: {order.CustomerId}\nOrder date: {order.OrderDate}\nOrder price: {order.OrderPrice}\n");

                    Console.WriteLine("\nAre you sure you wish to delete your current order? ");
                    Console.Write("[Y]es / [N]o: ");
                    var answer = Console.ReadLine()!;
                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var result = _orderService.DeleteOrder(order);
                        SubMenuTemplate("Delete Status");
                        if (result != null)
                        {
                            Console.WriteLine("Order deleted:");
                            Console.WriteLine($"\nId: {result.Id}\nCustomer Id: {result.CustomerId}\nOrder date: {result.OrderDate}\nOrder price: {result.OrderPrice}\n");
                        }
                    }
                    else
                        Console.WriteLine("Order will not be deleted.");
                }
                else
                    Console.WriteLine("No Orders associated with user, or no user logged in.");

                PressKeyAndContinue();
            }

            void ShowCreateOrderRowMenu()
            {
                var orderRow = new OrderRowDto();

                var products = _productService.GetAllProducts();
                SubMenuTemplate("Create Order row");
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        Console.WriteLine($"" +
                            $"Article number: {"",-4}{product.ArticleNumber}\n" +
                            $"Title: {"",-13}{product.Title}\n" +
                            $"Ingress: {"",-11}{product.Ingress}\n" +
                            $"Description: {"",-7}{product.Description}\n" +
                            $"Category: {"",-10}{product.CategoryName}\n" +
                            $"Price: {"",-13}{product.Price} {product.Currency}\n" +
                            $"Discount price: {"",-4}{product.DiscountPrice} {product.Currency}\n" +
                            $"Unit: {"",-14}{product.Unit}\n" +
                            $"Stock: {"",-13}{product.Stock}\n");
                    }
                    Console.WriteLine("\nFill in article number and quantity of the product you want to purchase.\n");

                    Console.Write("\nArticle Number*: ");
                    orderRow.ArticleNumber = Console.ReadLine()!;

                    Console.Write("\nQuantity*: ");
                    var quantityResult = int.TryParse(Console.ReadLine()!, out int quantity);
                    if (quantityResult && quantity >= 1)
                    {
                        orderRow.Quantity = quantity;

                        var result = _orderRowService.CreateOrderRow(orderRow);
                        if (result != null)
                        {
                            SubMenuTemplate("New Order row created:");
                            Console.WriteLine($"" +
                                $"Id: {"",-12}{result.Id}\n" +
                                $"Article number: {result.ArticleNumber}\n" +
                                $"Order Id: {"",-7}{result.OrderId}\n" +
                                $"Quantity: {"",-6}{result.Quantity}\n" +
                                $"Order Row Price: {"",-1}{result.OrderRowPrice}\n");
                        }
                        else
                        {
                            SubMenuTemplate("Order row status:");
                            Console.WriteLine($"Order row could not be created or already exists!\nIf you wish to add more items to your order the use update order rows instead.");
                        }
                    }
                    else
                        Console.WriteLine("Order row could not be created, please try again.");
                }
                else
                    Console.WriteLine("There are curently no products to purchase, create a new product from the 'Create Product Menu.'");

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
                        Console.WriteLine($"" +
                            $"Id: {"",-12}{orderRow.Id}\n" +
                            $"Article number: {orderRow.ArticleNumber}\n" +
                            $"Order Id: {"",-7}{orderRow.OrderId}\n" +
                            $"Quantity: {"",-6}{orderRow.Quantity}\n" +
                            $"Order Row Price: {"",-1}{orderRow.OrderRowPrice}\n");
                    }
                }
                else
                    Console.WriteLine("No Order rows currently associated with user");

                PressKeyAndContinue();
            }

            void ShowUpdateOrderRow()
            {
                var updatedOrderRow = new OrderRowDto();

                var orderRows = _orderRowService.GetAllOrderRows();
                SubMenuTemplate("Update Order row:");
                foreach (var orderRow in orderRows)
                {
                    Console.WriteLine($"" +
                        $"Id: {"",-12}{orderRow.Id}\n" +
                        $"Article number: {orderRow.ArticleNumber}\n" +
                        $"Order Id: {"",-7}{orderRow.OrderId}\n" +
                        $"Quantity: {"",-6}{orderRow.Quantity}\n" +
                        $"Order Row Price: {"",-1}{orderRow.OrderRowPrice}\n");
                }

                Console.WriteLine("Please fill in Id of order to update:");
                Console.Write("Id: ");
                var orderRowId = int.TryParse(Console.ReadLine()!, out int id);
                if (orderRowId || id >= 1)
                {
                    updatedOrderRow.Id = id;

                    Console.Write("Fill in updated quantity for order row: ");
                    var orderRowQuantity = int.TryParse(Console.ReadLine()!, out int quantity);
                    if (orderRowQuantity || quantity >= 1)
                    {
                        updatedOrderRow.Quantity = quantity;
                        var result = _orderRowService.UpdateOrderRow(updatedOrderRow);
                        if (result != null)
                        {
                            SubMenuTemplate("Order row updated to: ");
                            Console.WriteLine($"" +
                                $"Id: {"",-12}{result.Id}\n" +
                                $"Article number: {result.ArticleNumber}\n" +
                                $"Order Id: {"",-7}{result.OrderId}\n" +
                                $"Quantity: {"",-6}{result.Quantity}\n" +
                                $"Order Row Price: {"",-1}{result.OrderRowPrice}\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Could not find order row, please try again.");
                }

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
                        Console.WriteLine($"" +
                            $"Id: {"",-12}{orderRow.Id}\n" +
                            $"Article number: {orderRow.ArticleNumber}\n" +
                            $"Order Id: {"",-7}{orderRow.OrderId}\n" +
                            $"Quantity: {"",-6}{orderRow.Quantity}\n" +
                            $"Order Row Price: {"",-1}{orderRow.OrderRowPrice}\n");
                    }

                    Console.WriteLine("\nType in Id of the order row to delete: ");
                    Console.Write("Id: ");
                    var answer = int.TryParse(Console.ReadLine()!, out int Id);
                    if (answer)
                    {
                        order.Id = Id;

                        var result = _orderRowService.DeleteOrderRow(order);
                        SubMenuTemplate("Delete Status");
                        if (result != null)
                        {
                            Console.WriteLine("Order row deleted:");
                            Console.WriteLine($"" +
                                $"Id: {"",-12}{result.Id}\n" +
                                $"Article number: {result.ArticleNumber}\n" +
                                $"Order Id: {"",-7}{result.OrderId}\n" +
                                $"Quantity: {"",-6}{result.Quantity}\n" +
                                $"Order Row Price: {"",-1}{result.OrderRowPrice}\n");
                        }
                        else
                            Console.WriteLine("Something went wrong, order row was not be deleted.");
                    }
                    else
                        Console.WriteLine("Something went wrong, no order row by that id.");
                }
                else
                    Console.WriteLine("No order rows currently associated with user");

                PressKeyAndContinue();
            }
        }

        void ShowUserSettingsMenu()
        {
            bool userMenuLoop = true;

            while (userMenuLoop)
            {
                Console.Clear();
                Console.WriteLine($"{"",-5}User settings menu - Choose an option");
                string hyphens = new string('-', $"{"",5}User settings menu - Choose an option{"",-5}".Length);
                Console.WriteLine(hyphens);
                Console.WriteLine($"{"\n1.",-5} Show User options");
                Console.WriteLine($"{"\n2.",-5} Show Customer options");
                Console.WriteLine($"{"\n3.",-5} Show Address options");
                Console.WriteLine($"{"\n4.",-5} Show Customer_Adress Options");
                Console.WriteLine($"{"\n0.",-5} Go back");
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
                        Console.Clear();
                        Console.WriteLine($"{"",-5}User menu - Choose an option");
                        string hyphens = new string('-', $"{"",5}User menu - Choose an option{"",-5}".Length);
                        Console.WriteLine(hyphens);
                        Console.WriteLine($"{"\n1.",-5} Register new User (creates user, customer and address)");
                        Console.WriteLine($"{"\n2.",-5} Login User");
                        Console.WriteLine($"{"\n3.",-5} Logout User");
                        Console.WriteLine($"{"\n4.",-5} Show User details (requires said user or Admin to be logged in)");
                        Console.WriteLine($"{"\n5.",-5} Update User (requires said user or Admin to be logged in)");
                        Console.WriteLine($"{"\n6.",-5} Delete User (requires said or Admin to be logged in)");
                        Console.WriteLine($"{"\n0.",-5} Go back");
                        Console.Write($"\n\n{"",-5}Option: ");

                        var option = Console.ReadLine();

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
                            Console.WriteLine($"\n{"",-14}{user.StreetName}\n{"",-14}{user.PostalCode}\n{"",-14}{user.City}\n{"",-14}{user.Country}");

                            Console.Write("\n[Y]es / [N]o: ");
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

                        void ShowUserLoginMenu()
                        {
                            UserDto user = new();

                            SubMenuTemplate("Login user");

                            Console.Write("\nEmail: ");
                            user.Email = Console.ReadLine()!;

                            Console.Write("Password: ");
                            user.Password = Console.ReadLine()!;
                            SubMenuTemplate("Login status");
                            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                            {
                                Console.WriteLine("\nError, user could not be logged in - Please make sure no fields are blank and try again");
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

                        void ShowUserLogoutMenu()
                        {
                            var result = _userService.LogOutActiveUser();
                            SubMenuTemplate("Logout status");
                            if (result)
                            {
                                Console.WriteLine($"\nUser was succesfully logged out.");
                            }
                            else
                                Console.WriteLine("\nLogout attempt failed - email not recognized / no users currently logged in.");

                            PressKeyAndContinue();
                        }

                        void ShowActiveUserDetailsMenu()
                        {
                            UserDto userDto = new();

                            SubMenuTemplate("Show User details");

                            Console.Write("\nEmail: ");
                            userDto.Email = Console.ReadLine()!;

                            var result = _userService.GetOne(userDto);
                            SubMenuTemplate("User details result:");
                            if (result != null)
                            {
                                Console.WriteLine($"\nId:{"",-17}{result.Id}\nEmail:{"",-14}{result.Email}\nLogged in:{"",-10}{result.isActive}\nAccount created:{"",-4}{result.Created}\n");
                            }
                            else
                                Console.WriteLine("No user found with that email.");

                            PressKeyAndContinue();
                        }

                        void ShowUserUpdateMenu()
                        {
                            UserDto existingUser = new();
                            UserDto newUserDetails = new();

                            SubMenuTemplate("Update user - fill in email of the user to update");

                            Console.Write("\nEmail: ");
                            existingUser.Email = Console.ReadLine()!;

                            SubMenuTemplate($"Update {existingUser.Email} - fill in new user details");
                            Console.WriteLine("\nAny fields left empty will keep their current value\n");

                            Console.Write("Email: ");
                            newUserDetails.Email = Console.ReadLine()!;

                            Console.Write("Password: ");
                            newUserDetails.Password = Console.ReadLine()!;

                            var existingUserRoles = _userRoleService.GetAll();
                            if (existingUserRoles.Count() >= 1)
                            {
                                foreach (var role in existingUserRoles)
                                {
                                    Console.WriteLine($"\n" +
                                        $"Role Id: {role.Id}\n" +
                                        $"Role name: {role.RoleName}");
                                }

                            }
                            Console.WriteLine("Please select the Id of the Role you wish to update to");
                            Console.Write("Role Id: ");
                            var idResult = int.TryParse(Console.ReadLine()!, out int id);
                            if (idResult)
                            {
                                newUserDetails.UserRoleId = id;
                            }

                            var result = _userService.UpdateUser(existingUser, newUserDetails);
                            SubMenuTemplate("Update status");
                            if (result != null)
                            {
                                Console.WriteLine($"\nUser was updated to:\n\nId:{"",-5}{result.Id}\n\nEmail:{"",-2}{result.Email}\n\nIf you made changes to your password please store it in a secure location.");
                            }
                            else
                            {
                                Console.WriteLine($"\n{existingUser.Email} could not be updated!\nPlease make sure {existingUser.Email} is logged in and is a valid email.");
                            }

                            PressKeyAndContinue();
                        }

                        void ShowUserDeleteMenu()
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
                                Console.WriteLine($"\nId:{"",-12}{deleteCheck.Id}\nEmail:{"",-9}{deleteCheck.Email}\nRole: {"",-9}{deleteCheck.UserRole.RoleName}");
                                Console.Write($"\nIs this the user you wish to delete?\n[Y]es / [N]o: ");
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

                void ShowCustomerOptionsMenu() // Feels finished unless something big changes.
                {
                    bool customerLoop = true;
                    while (customerLoop)
                    {
                        Console.Clear();
                        Console.WriteLine($"{"",-5}Customer menu - Choose an option");
                        string hyphens = new string('-', $"{"",5}Customer menu - Choose an option".Length);
                        Console.WriteLine(hyphens);
                        Console.WriteLine($"{"\n1.",-5} Show Customer details");
                        Console.WriteLine($"{"\n2.",-5} Show all Customers");
                        Console.WriteLine($"{"\n3.",-5} Update Customer (requires user to be logged in)");
                        //Console.WriteLine($"{"\n4.",-5} Delete Customer (requires user to be logged in)");
                        Console.WriteLine($"{"\n0.",-5} Go back");
                        Console.Write($"\n\n{"",-5}Option: ");
                        var option = Console.ReadLine();

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
                            //case "4":
                            //    ShowDeleteCustomer();
                            //    break;
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

                        Console.Write("\nPlease enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        var customerDetails = _customerService.GetOneCustomerWithDetails(customer);
                        SubMenuTemplate("Customer details");
                        if (customerDetails.userRole != null && customerDetails.customer != null)
                        {
                            Console.WriteLine($"\nId: {"",-8}{customerDetails.customer.Id}\nFirst name: {"",-0}{customerDetails.customer.FirstName}\nLast name: {"",-1}{customerDetails.customer.LastName}\nEmail: {"",-5}{customerDetails.customer.EmailId}\nPhone no: {"",-2}{customerDetails.customer.PhoneNumber}");
                            Console.WriteLine($"Role: {"",-6}{customerDetails.userRole.RoleName}\n");
                            Console.WriteLine($"\nList of addresses associated with {customerDetails.Item3.FirstName} {customerDetails.Item3.LastName}:");

                            if (customerDetails.address != null)
                            {
                                var i = 1;
                                foreach (var address in customerDetails.address)
                                {
                                    Console.WriteLine($"\n{i++}{".",-11}{address.StreetName}\n{"",-12}{address.PostalCode}\n{"",-12}{address.City}\n{"",-12}{address.Country}");
                                }
                            }
                            else
                                Console.WriteLine("\nThere are currently no addresses linked to this customer");
                        }
                        else
                        {
                            SubMenuTemplate("Error - No customer details found!");
                            Console.WriteLine($"\nThere are currently no customers associated with {customer.EmailId}");
                        }

                        PressKeyAndContinue();
                    }

                    void ShowAllCustomers()
                    {
                        var allCustomers = _customerService.GetAll();

                        SubMenuTemplate("All current customers");
                        if (allCustomers.Any())
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

                    void ShowUpdateCustomer()
                    {
                        CustomerDto newCustomerDetails = new();

                        SubMenuTemplate("Update customer");
                        Console.WriteLine("\nFill in email of customer to update:");
                        Console.Write("\nEmail: ");

                        newCustomerDetails.EmailId = Console.ReadLine()!;
                        var exisitingCustomer = _customerService.GetOneCustomer(newCustomerDetails);

                        SubMenuTemplate("Update status");
                        Console.WriteLine($"\n{exisitingCustomer.Id}\n{exisitingCustomer.FirstName} {exisitingCustomer.LastName}\n{exisitingCustomer.EmailId}\n{exisitingCustomer.PhoneNumber}\n");
                        var customerAnswer = Question("Is this the customer you wish to update");
                        if (customerAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                        {
                            SubMenuTemplate($"Update {exisitingCustomer.EmailId} - fill in new customer details.");
                            Console.WriteLine("\nAny fields left empty will keep their current value.");
                            Console.Write("\nFirst name: ");
                            newCustomerDetails.FirstName = Console.ReadLine()!;
                            Console.Write("\nLast name: ");
                            newCustomerDetails.LastName = Console.ReadLine()!;
                            Console.Write("\nPhone number: ");
                            newCustomerDetails.PhoneNumber = Console.ReadLine()!;

                            Console.WriteLine(
                                $"\n{exisitingCustomer.Id}" +
                                $"\n{exisitingCustomer.EmailId}" +
                                $"\n{(!string.IsNullOrWhiteSpace(newCustomerDetails.FirstName) ? newCustomerDetails.FirstName : exisitingCustomer.FirstName)}" +
                                $"\n{(!string.IsNullOrWhiteSpace(newCustomerDetails.LastName) ? newCustomerDetails.LastName : exisitingCustomer.LastName)}" +
                                $"\n{(!string.IsNullOrWhiteSpace(newCustomerDetails.PhoneNumber) ? newCustomerDetails.PhoneNumber : exisitingCustomer.PhoneNumber)}");

                            Console.WriteLine("\nDo you want to update customer with these details? ");
                            var updateAnswer = Question("Continue with update?");
                            if (updateAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var customerResult = _customerService.UpdateCustomer(exisitingCustomer, newCustomerDetails);
                                if (customerResult != null)
                                {
                                    Console.WriteLine($"\nCustomer updated succesfully\n\nNew Customer details:\nId: {customerResult.Id}\nFirst name: {customerResult.FirstName}\nLast name: {customerResult.LastName}\nEmail: {customerResult.EmailId}\nPhone number: {customerResult.PhoneNumber}"); // add feedback as to new user details.
                                }
                                else
                                    Console.WriteLine("\nCustomer failed to update, please try again - if issue persists contact support.");
                            }
                            else
                                Console.WriteLine("\nCustomer was not updated");
                        }
                        else
                            Console.WriteLine("\nCustomer will not be updated");

                        PressKeyAndContinue();

                    }

                    //void ShowDeleteCustomer() // Not sure if this shoukd be able to be deleted, would rather the User be deleted, or the customer updated.
                    //{
                    //    SubMenuTemplate("Delete customer - Please choose an option");

                    //    CustomerDto customer = new();

                    //    Console.WriteLine("\nPlease choose an option.");
                    //    Console.WriteLine("\n1. (Find by email).");
                    //    Console.WriteLine("\n2. (Find by ID).");
                    //    Console.WriteLine("\n3. (Find by Phone number).");

                    //    Console.Write("Option: ");
                    //    var answer = Console.ReadLine()!;
                    //    customer = OptionsSwitch(answer);

                    //    var result = _customerService.DeleteCustomer(customer, answer);
                    //    SubMenuTemplate("Delete status");
                    //    if (result)
                    //    {
                    //        Console.WriteLine($"Id: {customer.Id}\n {customer.FirstName} {customer.LastName}\nEmail: {customer.EmailId}\nPhone Number: {customer.PhoneNumber}Has been deleted.");
                    //    }
                    //    else
                    //        Console.WriteLine($"Customer could not be deleted");

                    //    PressKeyAndContinue();
                    //}
                }

                void ShowAddressOptionsMenu()
                {
                    bool addressloop = true;

                    while (addressloop)
                    {
                        Console.Clear();
                        Console.WriteLine($"{"",-5}Address menu - Choose an option");
                        string hyphens = new string('-', $"{"",5}Adress menu - Choose an option".Length);
                        Console.WriteLine(hyphens);
                        Console.WriteLine($"{"\n1.",-5} Create Address (requires user to be logged in)");
                        Console.WriteLine($"{"\n2.",-5} Show Address details");
                        Console.WriteLine($"{"\n3.",-5} Show all Addresses");
                        Console.WriteLine($"{"\n4.",-5} Update Address (requires Admin privileges)");
                        Console.WriteLine($"{"\n5.",-5} Delete Address (requires Admin privileges)");
                        Console.WriteLine($"{"\n0.",-5} Go back");
                        Console.Write($"\n\n{"",-5}Option: ");
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
                            }
                            else
                                Console.WriteLine("Address could not be created, please try again.");
                        }
                        else
                            Console.WriteLine("Address was not created.");

                        PressKeyAndContinue();
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

                        var addressDetails = _addressService.GetOneAddressWithCustomers(address);
                        SubMenuTemplate("Address details");
                        if (addressDetails.address != null)
                        {
                            Console.WriteLine($"\nStreet name: {addressDetails.address.StreetName}\nPostal code: {addressDetails.address.PostalCode}\nCity: {addressDetails.address.City}\nCountry: {addressDetails.address.Country}\n");

                            Console.WriteLine($"\nList of customers associated with {addressDetails.address.StreetName}, {addressDetails.address.PostalCode}:");

                            if (addressDetails.customers != null)
                            {
                                var i = 1;
                                foreach (var customer in addressDetails.customers)
                                {
                                    Console.WriteLine($"\n{i++}{".",-4}{customer.FirstName} {customer.LastName}\n{"",-5}{customer.Id}\n{"",-5}{customer.EmailId}\n{"",-5}{customer.PhoneNumber}");
                                }
                            }
                            else
                                Console.WriteLine("\nThere are currently no customers linked to this Address");
                        }
                        else
                            Console.WriteLine($"\nThere are currently no addresses linked to {address.StreetName}, {address.PostalCode}");

                        PressKeyAndContinue();
                    }

                    void ShowAllAddresses()
                    {
                        var allAddresses = _addressService.GetAll();

                        SubMenuTemplate("All current addresses");

                        var i = 1;
                        foreach (var address in allAddresses)
                        {
                            Console.WriteLine($"\n{i++}{".",-4}{address.StreetName}\n{"",-5}{address.PostalCode}\n{"",-5}{address.City}\n{"",-5}{address.Country}");
                        }
                        PressKeyAndContinue();
                    }

                    void ShowUpdateAddress() // Should this be an accessible function for users or only admins? what if an address has multiple users and one of them wants to change it, better they create a new one?
                    {
                        AddressDto address = new();
                        AddressDto newAddressDetails = new();

                        SubMenuTemplate("Get address details");
                        Console.Write("\nPlease enter Street name and postal code of Address: \n");

                        Console.Write("\nStreet name: ");
                        address.StreetName = Console.ReadLine()!;
                        Console.Write("Postal Code: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingAddress = _addressService.GetOneAddress(address);
                        if (existingAddress != null)
                        {
                            SubMenuTemplate("Update status");
                            Console.WriteLine($"\n{existingAddress.StreetName}\n{existingAddress.PostalCode}\n{existingAddress.City}\n{existingAddress.Country}\n");
                            Console.Write("Is this the address you wish to update? ");
                            var answer = Console.ReadLine()!;
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                SubMenuTemplate($"Update {existingAddress.StreetName}, {existingAddress.PostalCode} - fill in new address details.");
                                Console.WriteLine("\nAny fields left empty will keep their current value.");
                                Console.Write("\nStreet name: ");
                                newAddressDetails.StreetName = Console.ReadLine()!;
                                Console.Write("\nPostal code: ");
                                newAddressDetails.PostalCode = Console.ReadLine()!;
                                Console.Write("\nCity: ");
                                newAddressDetails.City = Console.ReadLine()!;
                                Console.Write("\nCountry: ");
                                newAddressDetails.Country = Console.ReadLine()!;

                                Console.WriteLine($"" +
                                    $"\n{(string.IsNullOrWhiteSpace(newAddressDetails.StreetName) ? existingAddress.StreetName : newAddressDetails.StreetName)}" +
                                    $"\n{(string.IsNullOrWhiteSpace(newAddressDetails.PostalCode) ? existingAddress.PostalCode : newAddressDetails.PostalCode)}" +
                                    $"\n{(string.IsNullOrWhiteSpace(newAddressDetails.City) ? existingAddress.City : newAddressDetails.City)}" +
                                    $"\n{(string.IsNullOrWhiteSpace(newAddressDetails.Country) ? existingAddress.Country : newAddressDetails.Country)}\n");

                                Console.WriteLine("\nDo you want to update address with these details? ");
                                Console.Write("Continue with update? ");
                                var updateAnswer = Console.ReadLine()!;
                                if (updateAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var addressResult = _addressService.UpdateAddress(existingAddress, newAddressDetails);
                                    SubMenuTemplate("Update status");
                                    if (addressResult != null)
                                    {
                                        Console.WriteLine($"\nAddress updated succesfully\n\nNew address details:\nStreet name: {addressResult.StreetName}\nPoastal code: {addressResult.PostalCode}\nCity: {addressResult.City}\nCountry: {addressResult.Country}\n");
                                    }
                                    else
                                        Console.WriteLine("Address failed to update, ensure you are logged in as Admin.");
                                }
                                else
                                    Console.WriteLine("Address will not be updated");
                            }
                            else
                                Console.WriteLine("Address will not be updated");
                        }
                        else
                            Console.WriteLine("Address not found.\nMake sure all fields are filled in with valid details.");

                        PressKeyAndContinue();
                    }

                    void ShowDeleteAddress()
                    {
                        AddressDto address = new();

                        SubMenuTemplate("Delete Address");
                        Console.Write("\nFill in Street name and postal code of address to delete: \n");

                        Console.Write("Street name: ");
                        address.StreetName = Console.ReadLine()!;
                        Console.Write("Postal Code: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingAddress = _addressService.GetOneAddress(address);
                        SubMenuTemplate("Delete status");
                        if (existingAddress != null)
                        {
                            Console.WriteLine($"\n{existingAddress.StreetName}\n{existingAddress.PostalCode}\n{existingAddress.City}\n{existingAddress.Country}\n");
                            Console.WriteLine("Is this the address you wish to delete? This action can not be undone.");
                            Console.Write("[Y]es / [N]o:");

                            var answer = Console.ReadLine()!;
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var addressResult = _addressService.DeleteAddress(existingAddress);
                                SubMenuTemplate("Delete status");
                                if (addressResult != null)
                                {
                                    Console.WriteLine($"\nAddress:\n\nStreet name: {addressResult.StreetName}\nPoastal code: {addressResult.PostalCode}\nCity: {addressResult.City}\nCountry: {addressResult.Country}\n\nWas succesfully deleted.");
                                }
                                else
                                    Console.WriteLine("Address failed to delete, ensure you are logged in as Admin.");
                            }
                            else
                                Console.WriteLine("Address will not be deleted.");

                        }
                        else
                            Console.WriteLine("Address not found.\nMake sure all fields are filled in with valid details.");

                        PressKeyAndContinue();
                    }
                }

                void ShowCustomer_AddressOptions()
                {
                    bool Customer_AddressLoop = true;

                    while (Customer_AddressLoop)
                    {
                        Console.Clear();
                        Console.WriteLine($"{"",-5}Customer Address menu - Choose an option");
                        string hyphens = new string('-', $"{"",5}Customer Adress menu - Choose an option".Length);
                        Console.WriteLine(hyphens);
                        Console.WriteLine($"{"\n1.",-5} Create Customer Address (requires user to be logged in)");
                        Console.WriteLine($"{"\n2.",-5} Show Customer Address details");
                        Console.WriteLine($"{"\n3.",-5} Show all Customer Addresses");
                        Console.WriteLine($"{"\n5.",-5} Delete Customer Address (requires Admin privileges)");
                        Console.WriteLine($"{"\n0.",-5} Go back");
                        Console.Write($"\n\n{"",-5}Option: ");
                        var option = Console.ReadLine();

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
                            case "5":
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

                        Console.Write("Please enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        Console.Write("Please enter Street name of address: ");
                        address.StreetName = Console.ReadLine()!;
                        Console.Write("Please enter postal code of address: ");
                        address.PostalCode = Console.ReadLine()!;

                        var result = _customer_addressService.CreateCustomer_Address(customer, address);
                        SubMenuTemplate("Customer_Address status");
                        if (result)
                        {
                            Console.WriteLine("Customer Address created.");
                        }
                        else
                            Console.WriteLine("Customer Address could not be created or already exists.");

                        PressKeyAndContinue();
                    }

                    void ShowGetCustomer_AddressDetails()
                    {
                        SubMenuTemplate("Fill in details of customer_address to view details");

                        CustomerDto customer = new();
                        AddressDto address = new();

                        Console.Write("Please enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        Console.Write("Please enter Street name of address: ");
                        address.StreetName = Console.ReadLine()!;
                        Console.Write("Please enter postal code of address: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingCustomer_Address = _customer_addressService.GetCustomer_Address(customer, address);
                        SubMenuTemplate("Customer Address");
                        if (existingCustomer_Address != null)
                        {
                            Console.WriteLine($"\n{"",-5}Customer:\n");
                            Console.WriteLine($"" +
                                $"{"",-5}{existingCustomer_Address.CustomerId}\n" +
                                $"{"",-5}{existingCustomer_Address.Customer.FirstName}\n" +
                                $"{"",-5}{existingCustomer_Address.Customer.LastName}\n" +
                                $"{"",-5}{existingCustomer_Address.Customer.EmailId}\n");

                            Console.WriteLine($"{"",-5}Address:\n");
                            Console.WriteLine($"" +
                                $"{"",-5}{existingCustomer_Address.AddressId}\n" +
                                $"{"",-5}{existingCustomer_Address.Address.StreetName}\n" +
                                $"{"",-5}{existingCustomer_Address.Address.PostalCode}\n" +
                                $"{"",-5}{existingCustomer_Address.Address.City}\n" +
                                $"{"",-5}{existingCustomer_Address.Address.Country}\n");
                        }
                        else
                            Console.WriteLine($"{"",-5}No Customer Address was found with those details.");

                        PressKeyAndContinue();
                    }

                    void ShowGetAllCustomer_AddressesMenu()
                    {
                        var existingCustomer_Addresses = _customer_addressService.GetAlLCustomer_Addresses();
                        if (existingCustomer_Addresses != null)
                        {

                            SubMenuTemplate("All current Customer Addresses");
                            foreach (Customer_AddressDto customer_Address in existingCustomer_Addresses)
                            {
                                Console.WriteLine($"\n" +
                                    $"{"",-5}Email:{"",-14}{customer_Address.Customer.EmailId}\n" +
                                    $"{"",-5}Customer Id:{"",-8}{customer_Address.CustomerId}\n" +
                                    $"\n" +
                                    $"{"",-5}Address Id:{"",-9}{customer_Address.AddressId}\n" +
                                    $"{"",-5}Street name:{"",-8}{customer_Address.Address.StreetName}\n" +
                                    $"{"",-5}Postal code:{"",-8}{customer_Address.Address.PostalCode}\n");
                            }
                        }
                        else
                            Console.WriteLine("There are currently no Customer Addresses.");

                        PressKeyAndContinue();
                    }

                    void ShowDeleteCustomer_AddressMenu()
                    {
                        SubMenuTemplate("Fill in details of customer_address to delete");

                        CustomerDto customer = new();
                        AddressDto address = new();

                        Console.Write("Please enter email of customer: ");
                        customer.EmailId = Console.ReadLine()!;

                        Console.Write("Please enter Street name of address: ");
                        address.StreetName = Console.ReadLine()!;
                        Console.Write("Please enter postal code of address: ");
                        address.PostalCode = Console.ReadLine()!;

                        var existingCustomer_Address = _customer_addressService.GetCustomer_Address(customer, address);
                        SubMenuTemplate("Customer Address");
                        if (existingCustomer_Address != null)
                        {
                            Console.WriteLine("Customer:");
                            Console.WriteLine($"" +
                                $"{existingCustomer_Address.CustomerId}" +
                                $"{existingCustomer_Address.Customer.FirstName}\n" +
                                $"{existingCustomer_Address.Customer.LastName}\n" +
                                $"{existingCustomer_Address.Customer.EmailId}\n");

                            Console.WriteLine("Address:");
                            Console.WriteLine($"" +
                                $"{existingCustomer_Address.AddressId}" +
                                $"{existingCustomer_Address.Address.StreetName}\n" +
                                $"{existingCustomer_Address.Address.PostalCode}\n" +
                                $"{existingCustomer_Address.Address.City}\n" +
                                $"{existingCustomer_Address.Address.Country}\n");

                            var answer = Question("Is this the Customer Address connection you wish to delete?");
                            SubMenuTemplate("Delete status");
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var deletedCustomer_Address = _customer_addressService.DeleteCustomer_Address(existingCustomer_Address);
                                SubMenuTemplate("Delete Status");
                                if (deletedCustomer_Address != null)
                                {
                                    Console.WriteLine("Customer:");
                                    Console.WriteLine($"" +
                                        $"{existingCustomer_Address.CustomerId}" +
                                        $"{existingCustomer_Address.Customer.FirstName}\n" +
                                        $"{existingCustomer_Address.Customer.LastName}\n" +
                                        $"{existingCustomer_Address.Customer.EmailId}\n");

                                    Console.WriteLine("Address:");
                                    Console.WriteLine($"" +
                                        $"{existingCustomer_Address.AddressId}" +
                                        $"{existingCustomer_Address.Address.StreetName}\n" +
                                        $"{existingCustomer_Address.Address.PostalCode}\n" +
                                        $"{existingCustomer_Address.Address.City}\n" +
                                        $"{existingCustomer_Address.Address.Country}\n");

                                    Console.WriteLine("Customer Address connecting these two entities has been deleted.");
                                }
                                else
                                    Console.WriteLine("Customer Address could not be deleted.");
                            }
                            else
                                Console.WriteLine("Customer Address will not be deleted.");
                        }
                        else
                            Console.WriteLine("No Customer Address was found with those details.");

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
                Console.Clear();
                Console.WriteLine($"{"",-5}Product menu - Choose an option");
                string hyphens = new string('-', $"{"",5}Product menu - Choose an option".Length);
                Console.WriteLine(hyphens);
                Console.WriteLine($"{"\n1.",-5} Create Product (requires Admin privileges)");
                Console.WriteLine($"{"\n2.",-5} Show Product details");
                Console.WriteLine($"{"\n3.",-5} Show all Product");
                Console.WriteLine($"{"\n4.",-5} Update Product (requires Admin privileges)");
                Console.WriteLine($"{"\n5.",-5} Delete Product (requires Admin privileges)");
                Console.WriteLine($"{"\n6.",-5} Price list Menu");
                Console.WriteLine($"{"\n7.",-5} Category Menu");
                Console.WriteLine($"{"\n0.",-5} Go back");

                Console.Write($"\n\n{"",-5}Option: ");
                var option = Console.ReadLine();

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

                Console.Write("\nArticle Number*: ");
                product.ArticleNumber = Console.ReadLine()!;

                Console.Write("\nProduct Title*: ");
                product.Title = Console.ReadLine()!;

                Console.Write("\nProduct Ingress: ");
                product.Ingress = Console.ReadLine()!;

                Console.Write("\nProduct description: ");
                product.Description = Console.ReadLine()!;

                Console.Write("\nCategory*: ");
                product.CategoryName = Console.ReadLine()!;

                Console.Write("\nUnit (sold as: each, pair, set of X, etc)*: ");
                product.Unit = Console.ReadLine()!;

                Console.Write("\nNumber of product items in stock*: ");
                var stockresult = int.TryParse(Console.ReadLine()!, out int stock);
                if (stockresult)
                {
                    product.Stock = stock;
                }
                else
                    product.Stock = 0;

                Console.Write("\nPrice*: ");
                var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                if (priceResult)
                {
                    product.Price = price;
                }
                else
                    product.Price = 0;

                Console.Write("\nCurrency (ie. SEK, USD, EUR)*: ");
                product.Currency = Console.ReadLine()!;

                Console.Write("\nDiscount price: ");
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
                        Console.WriteLine($"" +
                            $"Article number: {"",-4}{productDisplay.ArticleNumber}\n" +
                            $"Title: {"",-13}{productDisplay.Title}\n" +
                            $"Ingress: {"",-11}{productDisplay.Ingress}\n" +
                            $"Description: {"",-7}{productDisplay.Description}\n" +
                            $"Category: {"",-10}{productDisplay.CategoryName}\n" +
                            $"Price: {"",-13}{productDisplay.Price} {productDisplay.Currency}\n" +
                            $"Discount price: {"",-4}{productDisplay.DiscountPrice} {productDisplay.Currency}\n" +
                            $"Unit: {"",-14}{productDisplay.Unit}\n" +
                            $"Stock: {"",-13}{productDisplay.Stock}\n");
                    }
                    else
                    {
                        SubMenuTemplate("Product could not be created");
                    }

                    PressKeyAndContinue();
                }
            }

            void ShowProductDetailsMenu()
            {
                ProductDto productDto = new();
                SubMenuTemplate("Show product details");
                Console.Write("Fill in article number of product to show: ");
                productDto.ArticleNumber = Console.ReadLine()!;

                var product = _productService.GetProductDisplay(productDto);
                SubMenuTemplate("Product search results:");
                if (product != null)
                {

                    Console.WriteLine($"" +
                            $"{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                            $"{"",-5}Title: {"",-13}{product.Title}\n" +
                            $"{"",-5}Ingress: {"",-11}{product.Ingress}\n" +
                            $"{"",-5}Description: {"",-7}{product.Description}\n" +
                            $"{"",-5}Category: {"",-10}{product.CategoryName}\n" +
                            $"{"",-5}Price: {"",-13}{product.Price} {product.Currency}\n" +
                            $"{"",-5}Discount price: {"",-4}{product.DiscountPrice} {product.Currency}\n" +
                            $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                            $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                }
                else
                    Console.WriteLine("No product was found with that article number.");

                PressKeyAndContinue();
            }

            void ShowAllProductsMenu()
            {
                var products = _productService.GetAllProducts();
                SubMenuTemplate("All Products");
                if (products.Any())
                {
                    var i = 1;

                    foreach (var product in products)
                    {
                        string hyphens = new string('-', $"{i}.".Length);

                        Console.WriteLine($"" +
                            $"{i++}.\n" +
                            $"{hyphens}\n" +
                            $"{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                            $"{"",-5}Title: {"",-13}{product.Title}\n" +
                            $"{"",-5}Ingress: {"",-11}{product.Ingress}\n" +
                            $"{"",-5}Description: {"",-7}{product.Description}\n" +
                            $"{"",-5}Category: {"",-10}{product.CategoryName}\n" +
                            $"{"",-5}Price: {"",-13}{product.Price} {product.Currency}\n" +
                            $"{"",-5}Discount price: {"",-4}{product.DiscountPrice} {product.Currency}\n" +
                            $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                            $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                    }
                }
                else
                    Console.WriteLine("No products to show currently.");

                PressKeyAndContinue();
            }

            void ShowUpdateProductMenu()
            {
                ProductDto productDto = new();
                SubMenuTemplate("Update product details");
                Console.Write("Fill in article number of product to update: ");
                productDto.ArticleNumber = Console.ReadLine()!;

                var product = _productService.GetProductDisplay(productDto);
                if (product != null)
                {
                    SubMenuTemplate("Product search:");


                    Console.WriteLine($"\n" +
                            $"{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                            $"{"",-5}Title: {"",-13}{product.Title}\n" +
                            $"{"",-5}Ingress: {"",-11}{product.Ingress}\n" +
                            $"{"",-5}Description: {"",-7}{product.Description}\n" +
                            $"{"",-5}Category: {"",-10}{product.CategoryName}\n" +
                            $"{"",-5}Price: {"",-13}{product.Price} {product.Currency}\n" +
                            $"{"",-5}Discount price: {"",-4}{product.DiscountPrice} {product.Currency}\n" +
                            $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                            $"{"",-5}Stock: {"",-13}{product.Stock}\n");

                    Console.WriteLine("Is this the products to update?");
                    Console.Write("[Y]es / [N]o: ");
                    var answer = Console.ReadLine()!;
                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ProductRegistrationDto updatedProductDetails = new();

                        SubMenuTemplate("New product details:");
                        Console.WriteLine("\nFill in updated details of product." +
                            " Fields left empty will retain their current value.\n" +
                            "Please note that article number can not be changed, instead create a new product with a different article number.");

                        updatedProductDetails.ArticleNumber = product.ArticleNumber;

                        Console.Write("\nProduct Title: ");
                        updatedProductDetails.Title = Console.ReadLine()!;

                        Console.Write("\nProduct Ingress: ");
                        updatedProductDetails.Ingress = Console.ReadLine()!;

                        Console.Write("\nProduct description: ");
                        updatedProductDetails.Description = Console.ReadLine()!;

                        Console.Write("\nCategory: ");
                        updatedProductDetails.CategoryName = Console.ReadLine()!;

                        Console.Write("\nUnit (sold as: each, pair, set of X, etc): ");
                        updatedProductDetails.Unit = Console.ReadLine()!;

                        Console.Write("\nAdjust the number of product items in stock (prefix with - to reduce amount): ");
                        var stockresult = int.TryParse(Console.ReadLine()!, out int stock);
                        if (stockresult)
                        {
                            updatedProductDetails.Stock = stock;
                        }
                        else
                            updatedProductDetails.Stock = 0;

                        Console.Write("\nPrice: ");
                        var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                        if (priceResult)
                        {
                            updatedProductDetails.Price = price;
                        }
                        else
                            updatedProductDetails.Price = 0;

                        Console.Write("\nCurrency (ie. SEK, USD, EUR): ");
                        product.Currency = Console.ReadLine()!;

                        Console.Write("\nDiscount price: ");
                        var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                        if (discountPriceResult)
                        {
                            updatedProductDetails.DiscountPrice = discountPrice;
                        }
                        else
                            updatedProductDetails.DiscountPrice = 0;

                        var updatedProduct = _productService.UpdateProduct(updatedProductDetails);
                        if (updatedProduct != null)
                        {
                            var productDisplay = _productService.GetProductDisplay(updatedProduct);
                            if (productDisplay != null)
                            {
                                SubMenuTemplate("Product updated");
                                Console.WriteLine($"\n" +
                                    $"Article number: {"",-4}{productDisplay.ArticleNumber}\n" +
                                    $"Title: {"",-13}{productDisplay.Title}\n" +
                                    $"Ingress: {"",-11}{productDisplay.Ingress}\n" +
                                    $"Description: {"",-7}{productDisplay.Description}\n" +
                                    $"Category: {"",-10}{productDisplay.CategoryName}\n" +
                                    $"Price: {"",-13}{productDisplay.Price} {productDisplay.Currency}\n" +
                                    $"Discount price: {"",-4}{productDisplay.DiscountPrice} {productDisplay.Currency}\n" +
                                    $"Unit: {"",-14}{productDisplay.Unit}\n" +
                                    $"Stock: {"",-13}{productDisplay.Stock}\n");
                            }
                        }
                        else
                            Console.WriteLine("\nProduct could not be updated.");
                    }
                    else
                        Console.WriteLine("\nProduct will not be updated.");
                }
                else
                    Console.WriteLine("\nNo product found with that article number.");

                PressKeyAndContinue();
            }

            void ShowDeleteProductMenu()
            {
                ProductDto productDto = new();
                SubMenuTemplate("Delete product");
                Console.Write("\nFill in article number of product to delete: ");
                productDto.ArticleNumber = Console.ReadLine()!;

                var product = _productService.GetProductDisplay(productDto);
                SubMenuTemplate("Product search:");
                if (product != null)
                {
                    PrintProductDetails(product);

                    var answer = Question("Is this the product to delete?");
                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var result = _productService.DeleteProduct(productDto);
                        SubMenuTemplate("Delete Status:");
                        if (result != null)
                        {
                            Console.WriteLine($"\n" +
                                $"{"",-5}Article number: {"",-5}{result.ArticleNumber}\n" +
                                $"{"",-5}Title: {"",-13}{result.Title}\n" +
                                $"{"",-5}Ingress: {"",-11}{result.Ingress}\n" +
                                $"{"",-5}Category: {"",-10}{result.Category.CategoryName}\n");

                            Console.WriteLine("Was deleted.");
                        }
                        else
                            Console.WriteLine("\nProduct could not be deleted.");
                    }
                    else
                        Console.WriteLine("\nProduct will not be deleted.");
                }
                else
                    Console.WriteLine("\nThere was no product found with that article number.");

                PressKeyAndContinue();
            }

            void ShowPriceListMenu()
            {
                bool priceListLoop = true;

                while (priceListLoop)
                {
                    Console.Clear();
                    Console.WriteLine($"{"",-5}PriceList menu - Choose an option");
                    string hyphens = new string('-', $"{"",5}Product menu - Choose an option".Length);
                    Console.WriteLine(hyphens);
                    Console.WriteLine($"{"\n1.",-5} Create Price list (requires Admin privileges)");
                    Console.WriteLine($"{"\n2.",-5} Show Price list details");
                    Console.WriteLine($"{"\n3.",-5} Show all Price lists");
                    Console.WriteLine($"{"\n4.",-5} Update Price list (requires Admin privileges)");
                    Console.WriteLine($"{"\n5.",-5} Delete Price list (requires Admin privileges)");
                    Console.WriteLine($"{"\n0.",-5} Go back");

                    Console.Write($"\n\n{"",-5}Option: ");
                    var option = Console.ReadLine();

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
                    Console.WriteLine("Fill in details for new price list. Required fields are marked with *");

                    Console.Write("\nPrice *: ");
                    var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                    if (priceResult)
                    {
                        priceListDto.Price = price;
                    }

                    Console.Write("\nDiscount price: ");
                    var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                    if (discountPriceResult)
                    {
                        priceListDto.DiscountPrice = discountPrice;
                    }

                    Console.Write("\nUnit Type (ie. SEK, EUR, USD) *: ");
                    priceListDto.UnitType = Console.ReadLine()!;


                    SubMenuTemplate("Price list status");
                    Console.WriteLine(
                        $"\nPrice:{"",-14}{priceListDto.Price}" +
                        $"\nDiscount price:{"",-5}{priceListDto.DiscountPrice}" +
                        $"\nUnit type:{"",-10}{priceListDto.UnitType}");

                    var answer = Question("\nIs this the price list you want to create?");
                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var result = _priceListService.CreatePriceList(priceListDto);
                        SubMenuTemplate("Price list status");
                        if (result != null)
                        {
                            Console.WriteLine("\nPrice list created:");
                            Console.WriteLine(
                                $"\nId:{"",-17}{result.Id}" +
                                $"\nPrice:{"",-14}{result.Price}" +
                                $"\nDiscount price:{"",-5}{result.DiscountPrice}" +
                                $"\nUnit type:{"",-10}{result.UnitType}");
                        }
                        else
                            Console.WriteLine("Price list could not be created, make sure you are logged in as Admin.");
                    }
                    else
                        Console.WriteLine("Price list was not created.");

                    PressKeyAndContinue();
                }

                void ShowPriceListDetailsMenu()
                {
                    PriceListDto priceListDto = new();

                    var allPriceLists = _priceListService.GetAllPriceLists();
                    SubMenuTemplate("All current Price lists:");
                    if (allPriceLists != null)
                    {

                        foreach (var priceList in allPriceLists)
                        {
                            Console.WriteLine(
                                $"\nId:{"",-17}{priceList.Id}" +
                                $"\nPrice:{"",-14}{priceList.Price}" +
                                $"\nDiscount price:{"",-5}{priceList.DiscountPrice}" +
                                $"\nUnit type:{"",-10}{priceList.UnitType}");
                        }

                        Console.WriteLine("\n\nFill in Id of Price list to show details");

                        Console.Write("\nId: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            priceListDto.Id = Id;
                        }

                        var priceListDetails = _priceListService.GetPriceList(priceListDto);
                        SubMenuTemplate("Price list details");
                        if (priceListDetails != null)
                        {
                            Console.WriteLine("\nPrice list:");
                            Console.WriteLine(
                                $"\n{"",-5}Id:{"",-17}{priceListDetails.Id}" +
                                $"\n{"",-5}Price:{"",-14}{priceListDetails.Price}" +
                                $"\n{"",-5}Discount price:{"",-5}{priceListDetails.DiscountPrice}" +
                                $"\n{"",-5}Unit type:{"",-10}{priceListDetails.UnitType}\n");

                            Console.WriteLine("\nProducts currently associated to this Price list:");

                            foreach (var product in priceListDetails.Products)
                            {
                                Console.WriteLine(
                                    $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                                    $"{"",-5}Title: {"",-13}{product.Title}\n" +
                                    $"{"",-5}Category: {"",-10}{product.Category.CategoryName}\n" +
                                    $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                                    $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                            }
                        }
                        else
                            Console.WriteLine("No Price Lists with that Id.");
                    }
                    else
                        Console.WriteLine("No current Price Lists to show.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{priceList.Id}" +
                                $"\nPrice:{"",-14}{priceList.Price}" +
                                $"\nDiscount price:{"",-5}{priceList.DiscountPrice}" +
                                $"\nUnit type:{"",-10}{priceList.UnitType}");
                        }
                    }
                    else
                        Console.WriteLine("There are currently no Price lists.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{priceList.Id}" +
                                $"\nPrice:{"",-14}{priceList.Price}" +
                                $"\nDiscount price:{"",-5}{priceList.DiscountPrice}" +
                                $"\nUnit type:{"",-10}{priceList.UnitType}");
                        }

                        Console.WriteLine("\n\nFill in Id of Price list to update");

                        Console.Write("\nId: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            priceListDto.Id = Id;
                        }

                        var existingPriceList = _priceListService.GetPriceList(priceListDto);
                        SubMenuTemplate("Price list to update");
                        if (existingPriceList != null)
                        {
                            Console.WriteLine("\nPrice list:");
                            Console.WriteLine(
                                $"\n{"",-5}Id:{"",-17}{existingPriceList.Id}" +
                                $"\n{"",-5}Price:{"",-14}{existingPriceList.Price}" +
                                $"\n{"",-5}Discount price:{"",-5}{existingPriceList.DiscountPrice}" +
                                $"\n{"",-5}Unit type:{"",-10}{existingPriceList.UnitType}\n");

                            Console.WriteLine("\nProducts that will be affected by this update:");

                            foreach (var product in existingPriceList.Products)
                            {
                                Console.WriteLine(
                                    $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                                    $"{"",-5}Title: {"",-13}{product.Title}\n" +
                                    $"{"",-5}Category: {"",-10}{product.Category.CategoryName}\n" +
                                    $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                                    $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                            }

                            var answer = Question("\nDo you want to update this pricelist?");
                            SubMenuTemplate("Update Price list details");
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                PriceListDto updatedPricelistDetails = new();

                                Console.WriteLine("Fill in updated details for price list. Any fields left empty will keep their original values.");

                                updatedPricelistDetails.Id = existingPriceList.Id;

                                Console.Write("\nPrice: ");
                                var priceResult = decimal.TryParse(Console.ReadLine()!, out decimal price);
                                if (priceResult)
                                {
                                    updatedPricelistDetails.Price = price;
                                }

                                Console.Write("\nDiscount price: ");
                                var discountPriceResult = decimal.TryParse(Console.ReadLine()!, out decimal discountPrice);
                                if (discountPriceResult)
                                {
                                    updatedPricelistDetails.DiscountPrice = discountPrice;
                                }

                                Console.Write("\nUnit Type (ie. SEK, EUR, USD): ");
                                updatedPricelistDetails.UnitType = Console.ReadLine()!;

                                SubMenuTemplate("Confirm update");
                                Console.WriteLine("\nPrice list:");
                                Console.WriteLine(
                                    $"\n{"",-5}Id:{"",-17}{updatedPricelistDetails.Id}" +
                                    $"\n{"",-5}Price:{"",-14}{updatedPricelistDetails.Price}" +
                                    $"\n{"",-5}Discount price:{"",-5}{updatedPricelistDetails.DiscountPrice}" +
                                    $"\n{"",-5}Unit type:{"",-10}{updatedPricelistDetails.UnitType}\n");

                                var updateAnswer = Question("Do you wish to update Price list with these details?");
                                if (updateAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var updatedPriceList = _priceListService.UpdatePriceList(existingPriceList, updatedPricelistDetails);
                                    SubMenuTemplate("Update status");
                                    if (updatedPriceList != null)
                                    {
                                        Console.WriteLine("\nPrice list:");
                                        Console.WriteLine(
                                            $"\n{"",-5}Id:{"",-17}{updatedPriceList.Id}" +
                                            $"\n{"",-5}Price:{"",-14}{updatedPriceList.Price}" +
                                            $"\n{"",-5}Discount price:{"",-5}{updatedPriceList.DiscountPrice}" +
                                            $"\n{"",-5}Unit type:{"",-10}{updatedPriceList.UnitType}\n");

                                        Console.WriteLine("Price list succesfully updated.");
                                    }
                                    else
                                        Console.WriteLine("Price list could not be updated, make sure you are logged in as Admin.");
                                }
                                else
                                    Console.WriteLine("Price list was not updated.");
                            }
                            else
                                Console.WriteLine("Price List will not be updated.");
                        }
                        else
                            Console.WriteLine("No Price Lists with that Id.");
                    }
                    else
                        Console.WriteLine("No current Price Lists to show.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{priceList.Id}" +
                                $"\nPrice:{"",-14}{priceList.Price}" +
                                $"\nDiscount price:{"",-5}{priceList.DiscountPrice}" +
                                $"\nUnit type:{"",-10}{priceList.UnitType}");
                        }

                        Console.WriteLine("\n\nFill in Id of Price list to delete");

                        Console.Write("\nId: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            priceListDto.Id = Id;
                        }

                        var existingPriceList = _priceListService.GetPriceList(priceListDto);
                        SubMenuTemplate("Price list to delete");
                        if (existingPriceList != null)
                        {
                            Console.WriteLine("\nPrice list:");
                            Console.WriteLine(
                                $"\n{"",-5}Id:{"",-17}{existingPriceList.Id}" +
                                $"\n{"",-5}Price:{"",-14}{existingPriceList.Price}" +
                                $"\n{"",-5}Discount price:{"",-5}{existingPriceList.DiscountPrice}" +
                                $"\n{"",-5}Unit type:{"",-10}{existingPriceList.UnitType}\n");

                            Console.WriteLine("\nProducts that will be affected by this update:");

                            foreach (var product in existingPriceList.Products)
                            {
                                Console.WriteLine(
                                    $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                                    $"{"",-5}Title: {"",-13}{product.Title}\n" +
                                    $"{"",-5}Category: {"",-10}{product.Category.CategoryName}\n" +
                                    $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                                    $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                            }

                            var answer = Question("\nDo you want to delete this pricelist?");
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var deletedPriceList = _priceListService.DeletePriceList(existingPriceList);
                                if (deletedPriceList != null)
                                {
                                    SubMenuTemplate("Delete status");
                                    Console.WriteLine("\nPrice list:");
                                    Console.WriteLine(
                                        $"\n{"",-5}Id:{"",-17}{deletedPriceList.Id}" +
                                        $"\n{"",-5}Price:{"",-14}{deletedPriceList.Price}" +
                                        $"\n{"",-5}Discount price:{"",-5}{deletedPriceList.DiscountPrice}" +
                                        $"\n{"",-5}Unit type:{"",-10}{deletedPriceList.UnitType}\n");

                                    Console.WriteLine("Price list succesfully Deleted.");
                                }
                                else
                                    Console.WriteLine("Price list could not be deleted, make sure you are logged in as Admin.");
                            }
                            else
                                Console.WriteLine("Price will not be deleted.");
                        }
                        else
                            Console.WriteLine("No Price Lists with that Id.");
                    }
                    else
                        Console.WriteLine("No current Price Lists to show.");

                    PressKeyAndContinue();
                }

            }

            void ShowCategoryMenu()
            {
                bool CategoryLoop = true;

                while (CategoryLoop)
                {
                    Console.Clear();
                    Console.WriteLine($"{"",-5}Category menu - Choose an option");
                    string hyphens = new string('-', $"{"",5}Category menu - Choose an option".Length);
                    Console.WriteLine(hyphens);
                    Console.WriteLine($"{"\n1.",-5} Create Category (requires Admin privileges)");
                    Console.WriteLine($"{"\n2.",-5} Show Category details");
                    Console.WriteLine($"{"\n3.",-5} Show all Category");
                    Console.WriteLine($"{"\n4.",-5} Update Category (requires Admin privileges)");
                    Console.WriteLine($"{"\n5.",-5} Delete Category (requires Admin privileges)");
                    Console.WriteLine($"{"\n0.",-5} Go back");

                    Console.Write($"\n\n{"",-5}Option: ");
                    var option = Console.ReadLine();

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
                    Console.WriteLine("Fill in details for new Category. Required fields are marked with *");

                    Console.Write("\nCategory name *: ");
                    categoryDto.CategoryName = Console.ReadLine()!;


                    SubMenuTemplate("Category status");
                    Console.WriteLine(
                        $"\nCategory name:{"",-14}{categoryDto.CategoryName}");

                    var answer = Question("\nIs this the Category you want to create?");
                    if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var result = _categoryService.CreateCategory(categoryDto);
                        SubMenuTemplate("Category status");
                        if (result != null)
                        {
                            Console.WriteLine(
                                $"\nId:{"",-17}{result.Id}" +
                                $"\nCategory name:{"",-6}{result.CategoryName}");
                        }
                        else
                            Console.WriteLine("Category could not be created, make sure you are logged in as Admin.");
                    }
                    else
                        Console.WriteLine("Category was not created.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{category.Id}" +
                                $"\nCategory name:{"",-6}{category.CategoryName}");
                        }

                        Console.WriteLine("\n\nFill in Id of Category to show details");

                        Console.Write("\nId: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            categoryDto.Id = Id;
                        }

                        var categoryDetails = _categoryService.GetCategory(categoryDto);
                        SubMenuTemplate("Category details");
                        if (categoryDetails != null)
                        {
                            Console.WriteLine("\nCategory:");
                            Console.WriteLine(
                                     $"\nId:{"",-17}{categoryDetails.Id}" +
                                     $"\nCategory name:{"",-6}{categoryDetails.CategoryName}");

                            Console.WriteLine("\nProducts currently associated to this Category:");

                            foreach (var product in categoryDetails.Products)
                            {
                                Console.WriteLine(
                                    $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                                    $"{"",-5}Title: {"",-13}{product.Title}\n" +
                                    $"{"",-5}Category: {"",-10}{product.Category.CategoryName}\n" +
                                    $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                                    $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                            }
                        }
                        else
                            Console.WriteLine("No Category with that Id.");
                    }
                    else
                        Console.WriteLine("No current Categories to show.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{category.Id}" +
                                $"\nCategory name:{"",-6}{category.CategoryName}");
                        }
                    }
                    else
                        Console.WriteLine("\nThere are currently no Categories.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{category.Id}" +
                                $"\nCategory name:{"",-6}{category.CategoryName}");
                        }

                        Console.WriteLine("\n\nFill in Id of Category to update");

                        Console.Write("\nId: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            categoryDto.Id = Id;
                        }

                        var existingCategory = _categoryService.GetCategory(categoryDto);
                        SubMenuTemplate("Category to update");
                        if (existingCategory != null)
                        {
                            Console.WriteLine("\nCategory:");
                            Console.WriteLine(
                                $"\nId:{"",-17}{existingCategory.Id}" +
                                $"\nCategory name:{"",-6}{existingCategory.CategoryName}");

                            Console.WriteLine("\nProducts that will be affected by this update:");

                            foreach (var product in existingCategory.Products)
                            {
                                Console.WriteLine(
                                    $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                                    $"{"",-5}Title: {"",-13}{product.Title}\n" +
                                    $"{"",-5}Category: {"",-10}{product.Category.CategoryName}\n" +
                                    $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                                    $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                            }

                            var answer = Question("\nDo you want to update this Category?");
                            SubMenuTemplate("Update Category details");
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                CategoryDto updatedCategoryDetails = new();

                                Console.WriteLine("Fill in updated details for Category. Any fields left empty will keep their original values.");

                                Console.Write("\nCategory name: ");
                                updatedCategoryDetails.CategoryName = Console.ReadLine()!;

                                SubMenuTemplate("Confirm update");
                                Console.WriteLine("\nCategory:");
                                Console.WriteLine(
                                    $"\nId:{"",-17}{updatedCategoryDetails.Id}" +
                                    $"\nCategory name:{"",-6}{updatedCategoryDetails.CategoryName}");

                                var updateAnswer = Question("\nDo you wish to update Category with these details?");
                                if (updateAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var updatedCategory = _categoryService.UpdateCategory(existingCategory, updatedCategoryDetails);
                                    SubMenuTemplate("Update status");
                                    if (updatedCategory != null)
                                    {
                                        Console.WriteLine("\nPrice list:");
                                        Console.WriteLine(
                                            $"\nId:{"",-17}{updatedCategory.Id}" +
                                            $"\nCategory name:{"",-6}{updatedCategory.CategoryName}");

                                        Console.WriteLine("\nCategory succesfully updated.");
                                    }
                                    else
                                        Console.WriteLine("\nCategory could not be updated, make sure you are logged in as Admin.");
                                }
                                else
                                    Console.WriteLine("\nCategory was not updated.");
                            }
                            else
                                Console.WriteLine("\nCategory will not be updated.");
                        }
                        else
                            Console.WriteLine("\nNo Categories with that Id.");
                    }
                    else
                        Console.WriteLine("\nThere are currently no Categories.");

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
                            Console.WriteLine(
                                $"\nId:{"",-17}{category.Id}" +
                                $"\nCategory name:{"",-6}{category.CategoryName}");
                        }

                        Console.WriteLine("\n\nFill in Id of Category to delete");

                        Console.Write("\nId: ");
                        var idResult = int.TryParse(Console.ReadLine()!, out int Id);
                        if (idResult)
                        {
                            categoryDto.Id = Id;
                        }

                        var existingCategory = _categoryService.GetCategory(categoryDto);
                        SubMenuTemplate("Category to delete");
                        if (existingCategory != null)
                        {
                            Console.WriteLine("\nCategory:");
                            Console.WriteLine(
                                $"\nId:{"",-17}{existingCategory.Id}" +
                                $"\nCategory name:{"",-6}{existingCategory.CategoryName}");

                            Console.WriteLine("\nProducts that will be affected by this update:");

                            foreach (var product in existingCategory.Products)
                            {
                                Console.WriteLine(
                                    $"\n{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                                    $"{"",-5}Title: {"",-13}{product.Title}\n" +
                                    $"{"",-5}Category: {"",-10}{product.Category.CategoryName}\n" +
                                    $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                                    $"{"",-5}Stock: {"",-13}{product.Stock}\n");
                            }

                            var answer = Question("\nDo you want to delete this Category?");
                            SubMenuTemplate("Delete Category details");
                            if (answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var deletedCategory = _categoryService.DeleteCategory(existingCategory);
                                SubMenuTemplate("Update status");
                                if (deletedCategory != null)
                                {
                                    Console.WriteLine("\nPrice list:");
                                    Console.WriteLine(
                                        $"\nId:{"",-17}{deletedCategory.Id}" +
                                        $"\nCategory name:{"",-6}{deletedCategory.CategoryName}");

                                    Console.WriteLine("Category succesfully deleted.");
                                }
                                else
                                    Console.WriteLine("Category could not be deleted, make sure you are logged in as Admin.");
                            }
                            else
                                Console.WriteLine("Category will not be deleted.");
                        }
                        else
                            Console.WriteLine("NoCategory with that Id.");
                    }
                    else
                        Console.WriteLine("No current Categories to show.");

                    PressKeyAndContinue();
                }
            }

        }


        // Utility methods
        void PressKeyAndContinue()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        void SubMenuTemplate(string menuName)
        {
            Console.Clear();
            Console.WriteLine($"{"",-5}{menuName}");
            string hyphens = new string('-', $"{"",5}{menuName}{"",5}".Length);
            Console.WriteLine(hyphens);
        }

        string Question(string questionTextHere)
        {
            Console.WriteLine(questionTextHere);
            Console.Write("[Y]es / [N]o: ");
            var answer = Console.ReadLine()!;

            return answer;
        }

        void PrintProductDetails(ProductRegistrationDto product)
        {
            Console.WriteLine(
                $"\n" +
                $"{"",-5}Article number: {"",-4}{product.ArticleNumber}\n" +
                $"{"",-5}Title: {"",-13}{product.Title}\n" +
                $"{"",-5}Ingress: {"",-11}{product.Ingress}\n" +
                $"{"",-5}Description: {"",-7}{product.Description}\n" +
                $"{"",-5}Category: {"",-10}{product.CategoryName}\n" +
                $"{"",-5}Price: {"",-13}{product.Price} {product.Currency}\n" +
                $"{"",-5}Discount price: {"",-4}{product.DiscountPrice} {product.Currency}\n" +
                $"{"",-5}Unit: {"",-14}{product.Unit}\n" +
                $"{"",-5}Stock: {"",-13}{product.Stock}\n");
        }
    }
}