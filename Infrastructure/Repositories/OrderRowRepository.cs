using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class OrderRowRepository(ProductCatalog context) : BaseRepository<OrderRowEntity, ProductCatalog>(context), IOrderRowRepository
{
    private readonly ProductCatalog _context = context;

}

