using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;


namespace Business.Factories;

public static class OrderRowFactory
{
    public static OrderRowDto Create(OrderRowEntity entity)
    {
        try
        {
            return new OrderRowDto
            {
                Id = entity.Id,
                Quantity = entity.Quantity,
                ArticleNumber = entity.ArticleNumber,
                OrderRowPrice = entity.OrderRowPrice,
                OrderId = entity.OrderId,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static OrderRowEntity Create(OrderRowDto dto)
    {
        try
        {
            return new OrderRowEntity
            {
                Id = dto.Id,
                Quantity = dto.Quantity,
                ArticleNumber = dto.ArticleNumber,
                OrderRowPrice = dto.OrderRowPrice,
                OrderId = dto.OrderId,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<OrderRowDto> Create(IEnumerable<OrderRowEntity> entities)
    {
        try
        {
            List<OrderRowDto> list = new();

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

