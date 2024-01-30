
using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IUserRepository userRepository, UserService userService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _userService = userService;
    }

    public UserEntity GetActiveUser()
    {
        try
        {
            var isActive = _userRepository.GetOne(x => x.IsActive == true);
            if (isActive != null)
            {
                return isActive;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    // Setup so that customer can only have one order, if system had payment system check could be implemented to see if order was paid and then another order could be created fro istance.
    public OrderEntity CreateOrder()
    {
        try
        {
            var customerId = GetActiveUser();
            var existingOrder = _orderRepository.GetAllWithPredicate(x => x.CustomerId == customerId.Id);
            if (!existingOrder.Any())
            {
                OrderEntity order = new OrderEntity()
                {
                    OrderPrice = 0,
                    CustomerId = customerId.Id,
                    OrderDate = DateTime.Now,
                };

                var result = _orderRepository.Create(order);
                if (result != null)
                {
                    return result;
                }            
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<OrderDto> GetAllOrders()
    {
        try
        {
            var activeUser = GetActiveUser();
            var existingOrders = _orderRepository.GetAllWithPredicate(x => x.CustomerId == activeUser.Id);
            if (existingOrders != null)
            {
                return Factories.OrderFactory.Create(existingOrders);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public OrderDto GetOneOrder(OrderDto order)
    {
        try
        {  
            var activeUser = GetActiveUser();
            var existingOrder = _orderRepository.GetOne(x => x.Id == order.Id);
            if (existingOrder.CustomerId == activeUser.Id || activeUser.UserRoleName == "Admin")
            {
                return Factories.OrderFactory.Create(existingOrder);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public OrderDto UpdateOrder(OrderDto order)
    {
        try
        {
            var activeUser = GetActiveUser();
            var existingOrder = _orderRepository.GetOne(x => x.CustomerId == activeUser.Id && x.Id == order.Id); // if user is not logged in this will fail.

            if(existingOrder != null)  // Delete as logged in user
            {
                existingOrder.OrderPrice += order.OrderPrice;

                OrderEntity updatedOrder = new OrderEntity()
                {
                    Id = existingOrder.Id,
                    OrderPrice = existingOrder.OrderPrice,
                    CustomerId = activeUser.Id,
                    OrderDate = existingOrder.OrderDate,
                };
                
                var newOrderDetails = _orderRepository.Update(existingOrder, updatedOrder);
                if (newOrderDetails != null)
                {
                    return Factories.OrderFactory.Create(newOrderDetails);
                }
            }

            else if (activeUser.UserRoleName == "Admin") // Update as admin 
            {
                existingOrder = _orderRepository.GetOne(x => x.Id == order.Id); // only checks for order id not customerId so that Admin can use it.
                if (existingOrder != null)
                {
                    existingOrder.OrderPrice += order.OrderPrice;

                    OrderEntity updatedOrder = new OrderEntity()
                    {
                        Id = existingOrder.Id,
                        OrderPrice = order.OrderPrice,
                        CustomerId = activeUser.Id,
                        OrderDate = existingOrder.OrderDate,
                    };

                    var newOrderDetails = _orderRepository.Update(existingOrder, updatedOrder);
                    if (newOrderDetails != null)
                    {
                        return Factories.OrderFactory.Create(existingOrder);
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public OrderDto DeleteOrder(OrderDto order)
    {
        try
        {
            var activeUser = GetActiveUser();
            var existingOrder = _orderRepository.GetOne(x => x.CustomerId == activeUser.Id && x.Id == order.Id); // if user is not logged in this will fail.

            if (existingOrder != null) // Delete as logged in user
            {
                var result = _orderRepository.Delete(existingOrder);
                if (result)
                {
                    return Factories.OrderFactory.Create(existingOrder);
                }
            }
            else if (activeUser.UserRoleName == "Admin") // Delete as admin
            {
                existingOrder = _orderRepository.GetOne(x => x.Id == order.Id);  // only checks for order id not customerId so that Admin can use it.
                if (existingOrder != null)
                {
                    var result = _orderRepository.Delete(existingOrder);
                    if (result)
                    {
                        return Factories.OrderFactory.Create(existingOrder);
                    }
                }
            }
            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
