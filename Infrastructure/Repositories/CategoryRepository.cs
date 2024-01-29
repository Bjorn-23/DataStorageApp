using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class CategoryRepository(ProductCatalog context) : BaseRepository<CategoryEntity, ProductCatalog>(context), ICategoryRepository
{
    private readonly ProductCatalog _context = context;
}