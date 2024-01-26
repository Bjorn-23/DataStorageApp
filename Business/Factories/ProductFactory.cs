using Business.Dtos;
using Business.Services;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class ProductFactory
{
    public static ProductDto Create(ProductEntity entity)
    {
        try
        {
            return new ProductDto
            {
                ArticleNumber = entity.ArticleNumber,
                Title = entity.Title,
                Ingress = entity.Ingress,
                Description = entity.Description,
                PriceId = entity.PriceId,
                Unit = entity.Unit,
                Stock = entity.Stock,
                CategoryName = entity.CategoryName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static ProductEntity Create(ProductDto dto)
    {
        try
        {
            return new ProductEntity
            {
                ArticleNumber = dto.ArticleNumber,
                Title = dto.Title,
                Ingress = dto.Ingress,
                Description = dto.Description,
                PriceId = dto.PriceId,
                Unit = dto.Unit,
                Stock = dto.Stock,
                CategoryName = dto.CategoryName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static ProductRegistrationDto Create(ProductEntity entity, string UnitType, decimal Price, decimal? DiscountPrice)
    {
        try
        {
            return new ProductRegistrationDto()
            {
                ArticleNumber = entity.ArticleNumber,
                Title = entity.Title,
                Ingress = entity.Ingress,
                Description = entity.Description,
                Price = Price,
                Currency = UnitType,
                DiscountPrice = DiscountPrice,
                Unit = entity.Unit,
                Stock = entity.Stock,
                CategoryName = entity.CategoryName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
