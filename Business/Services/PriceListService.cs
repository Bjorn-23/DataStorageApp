using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class PriceListService
{
    private readonly IPriceListRepository _priceListRepository;

    public PriceListService(IPriceListRepository priceListRepository)
    {
        _priceListRepository = priceListRepository;
    }

    public PriceListEntity GetOrCreatePriceList(ProductRegistrationDto product)
    {
        try
        {            
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == product.Price && x.UnitType == product.Currency);
            if (existingPriceList == null && product.Price != 0)
            {
                var newPriceList = _priceListRepository.Create(new PriceListEntity()
                {
                    Price = product.Price,
                    DiscountPrice = product.DiscountPrice,
                    UnitType = product.Currency,
                });

                if (newPriceList != null)
                {
                    return newPriceList;
                }
            }
            if (existingPriceList != null)
            {
                return existingPriceList;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public PriceListDto GetOrCreatePriceList(PriceListDto priceList)
    {
        try
        {
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == priceList.Price && x.UnitType == priceList.UnitType);
            if (existingPriceList == null)
            {
                var entity = Factories.PriceListFactory.Create(priceList);
                var newPriceList = _priceListRepository.Create(entity);
                if (newPriceList != null)
                {
                    return Factories.PriceListFactory.Create(newPriceList);                      
                }
            }
            else
                return Factories.PriceListFactory.Create(existingPriceList);
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public (string UnitType, decimal Price, decimal? DiscountPrice) GetPriceList(ProductDto dto)
    {
        try
        {
            var existingPriceList = _priceListRepository.GetOne(x => x.Id == dto.PriceId);
            if (existingPriceList != null)
            {
                return (existingPriceList.UnitType, existingPriceList.Price, existingPriceList.DiscountPrice);
                
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return (null!, 0, 0);
    }

    public (string UnitType, decimal Price, decimal? DiscountPrice) GetPriceList(ProductEntity Entity)
    {
        try
        {
            var existingPriceList = _priceListRepository.GetOne(x => x.Id == Entity.PriceId);
            if (existingPriceList != null)
            {
                return (existingPriceList.UnitType, existingPriceList.Price, existingPriceList.DiscountPrice);

            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return (null!, 0, 0);
    }

}
