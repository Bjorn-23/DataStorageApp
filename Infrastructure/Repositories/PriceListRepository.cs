using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class PriceListRepository(ProductCatalog context) : BaseRepository<PriceListEntity, ProductCatalog>(context), IPriceListRepository
{
    private readonly ProductCatalog _context = context;

    public override PriceListEntity GetOne(Expression<Func<PriceListEntity, bool>> predicate)
    {
        try
        {
            var exisitingPriceListDetails = _context.PriceLists
                .Where(predicate)
                .Include(i => i.Products)
                .FirstOrDefault();

            if (exisitingPriceListDetails != null )
                return exisitingPriceListDetails;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}

