using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class PriceListFactory
{
    public static PriceListDto Create(PriceListEntity entity)
    {
        try
        {
            return new PriceListDto
            {
                Id = entity.Id,
                Price = entity.Price,
                DiscountPrice = entity.DiscountPrice,
                UnitType = entity.UnitType,
                Products = entity.Products
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static PriceListEntity Create(PriceListDto dto)
    {
        try
        {
            return new PriceListEntity
            {
                Id = dto.Id,
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                UnitType = dto.UnitType,
                Products = dto.Products
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<PriceListDto> Create(IEnumerable<PriceListEntity> entities)
    {
        try
        {
            List<PriceListDto> list = new();

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
