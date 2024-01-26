using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class PriceListRepository(ProductContext context) : BaseRepository<PriceListEntity, ProductContext>(context), IPriceListRepository
{
    private readonly ProductContext _context = context;
}

