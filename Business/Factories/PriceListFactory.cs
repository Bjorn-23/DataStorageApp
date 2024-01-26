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
                Price = entity.Price,
                DiscountPrice = entity.DiscountPrice,
                UnitType = entity.UnitType,
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
                Price = dto.Price,
                DiscountPrice = dto.DiscountPrice,
                UnitType = dto.UnitType,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
