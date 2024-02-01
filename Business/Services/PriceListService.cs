using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class PriceListService
{
    private readonly IPriceListRepository _priceListRepository;
    private readonly UserService _userService;

    public PriceListService(IPriceListRepository priceListRepository, UserService userService)
    {
        _priceListRepository = priceListRepository;
        _userService = userService;
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

    public PriceListDto CreatePriceList(PriceListDto priceList)
    {
        try
        {
            var activeUser = _userService.FindRoleOfActiveUser();
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == priceList.Price && x.UnitType == priceList.UnitType && x.DiscountPrice == priceList.DiscountPrice);
            if (existingPriceList == null && priceList.Price != 0 && activeUser.UserRoleName == "Admin")
            {
                var entity = PriceListFactory.Create(priceList);
                var newPriceList = _priceListRepository.Create(entity);
                if (newPriceList != null)
                {
                    return PriceListFactory.Create(newPriceList);                      
                }
            }            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public PriceListDto GetPriceList(PriceListDto priceList)
    {
        try
        {
            var existingPriceList = _priceListRepository.GetOne(x => x.Id == priceList.Id);
            if (existingPriceList != null)
            {
                return PriceListFactory.Create(existingPriceList);

            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<PriceListDto> GetAllPriceLists()
    {
        try
        {
            var existingPriceLists = _priceListRepository.GetAll();
            if (existingPriceLists != null)
            {
                return PriceListFactory.Create(existingPriceLists);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public PriceListDto UpdatePriceList(PriceListDto existingPriceListDto, PriceListDto updatedPriceListDto)
    {
        try
        {
            var activeUser = _userService.FindRoleOfActiveUser();
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == existingPriceListDto.Price && x.UnitType == existingPriceListDto.UnitType);
            if (existingPriceList != null && activeUser.UserRoleName == "Admin")
            {
                PriceListEntity updatedEntity = new()
                {
                    Id = existingPriceList.Id,
                    Price = updatedPriceListDto.Price <=0 ? existingPriceList.Price : updatedPriceListDto.Price,
                    DiscountPrice = !string.IsNullOrWhiteSpace(updatedPriceListDto.DiscountPrice.ToString()) ? updatedPriceListDto.DiscountPrice : existingPriceList.DiscountPrice,
                    UnitType = !string.IsNullOrWhiteSpace(updatedPriceListDto.UnitType) ? updatedPriceListDto.UnitType : existingPriceList.UnitType,
                };

                var updatedPriceList = _priceListRepository.Update(existingPriceList, updatedEntity);
                if (updatedPriceList != null)
                {
                    return PriceListFactory.Create(updatedPriceList);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public PriceListDto DeletePriceList(PriceListDto existingPriceListDto)
    {
        try
        {
            var activeUser = _userService.FindRoleOfActiveUser();
            var existingPriceList = _priceListRepository.GetOne(x => x.Price == existingPriceListDto.Price && x.UnitType == existingPriceListDto.UnitType);
            if (existingPriceList != null && activeUser.UserRoleName == "Admin")
            {
                var result = _priceListRepository.Delete(existingPriceList);
                if (result)
                {
                    return PriceListFactory.Create(existingPriceList);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
