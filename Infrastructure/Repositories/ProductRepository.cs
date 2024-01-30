using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class ProductRepository(ProductCatalog context) : BaseRepository<ProductEntity, ProductCatalog>(context), IProductRepository
{
    private readonly ProductCatalog _context = context;
}

