
using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
    }




    public UserEntity GetActiveCustomerToCreateOrder()
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

    public OrderEntity CreateOrder()
    {
        try
        {
            var customerId = GetActiveCustomerToCreateOrder();

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
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
