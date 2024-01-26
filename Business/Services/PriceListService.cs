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
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == product.Price && x.UnitType == product.UnitType);
            if (existingPriceList == null)
            {
                var newPriceList = _priceListRepository.Create(new PriceListEntity()
                {
                    Price = product.Price,
                    DiscountPrice = product.DiscountPrice,
                    UnitType = product.UnitType,
                });

                if (newPriceList != null)
                {
                    return newPriceList;
                }
            }
            else
                return existingPriceList;            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public PriceListDto GetOrCreatePriceList(PriceListDto priceList) // REFACTOR WITH FACTORIES!!
    {
        try
        {
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == priceList.Price && x.UnitType == priceList.UnitType);
            if (existingPriceList == null)
            {
                var newPriceList = _priceListRepository.Create(new PriceListEntity()
                {
                    Price = priceList.Price,
                    DiscountPrice = priceList.DiscountPrice,
                    UnitType = priceList.UnitType,
                });

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

}
