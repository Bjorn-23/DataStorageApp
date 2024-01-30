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

    public static OrderEntity Create(OrderDto dto)
    {
        try
        {
            return new OrderEntity
            {
                Id = dto.Id,
                OrderPrice = dto.OrderPrice,
                CustomerId = dto.CustomerId,
                OrderDate = dto.OrderDate,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<OrderDto> Create(IEnumerable<OrderEntity> entities)
    {
        try
        {
            List<OrderDto> list = new();

            foreach (var entity in entities)
            {
                list.Add(Create(entity));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
