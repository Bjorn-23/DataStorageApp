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

