using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class ProductRepository(ProductContext context) : BaseRepository<ProductEntity, ProductContext>(context), IProductRepository
{
    private readonly ProductContext _context = context;
}

