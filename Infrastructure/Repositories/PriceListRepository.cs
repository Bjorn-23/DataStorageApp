using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class PriceListRepository(ProductCatalog context) : BaseRepository<PriceListEntity, ProductCatalog>(context), IPriceListRepository
{
    private readonly ProductCatalog _context = context;
}

