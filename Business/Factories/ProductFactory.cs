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
                CategoryId = entity.CategoryId,
                Category = entity.Category,
                OrderRow = entity.OrderRow,
                Price = entity.Price,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static ProductRegistrationDto Create(ProductEntity entity, string UnitType, decimal Price, decimal? DiscountPrice) // Currently not used
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
                CategoryName = entity.Category.CategoryName

            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
