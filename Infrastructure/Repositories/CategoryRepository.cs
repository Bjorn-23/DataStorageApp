using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class CategoryRepository(ProductContext context) : BaseRepository<CategoryEntity, ProductContext>(context), ICategoryRepository
{
    private readonly ProductContext _context = context;
}