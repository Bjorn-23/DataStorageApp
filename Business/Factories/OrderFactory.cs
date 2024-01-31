using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class OrderFactory
{
    public static OrderDto Create(OrderEntity entity)
    {
        try
        {
            return new OrderDto
            {
                Id = entity.Id,
                OrderPrice = entity.OrderPrice,
                CustomerId = entity.CustomerId,
                OrderDate = entity.OrderDate,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
