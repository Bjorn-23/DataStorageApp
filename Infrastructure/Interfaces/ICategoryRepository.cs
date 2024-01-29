using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface ICategoryRepository : IBaseRepository<CategoryEntity, ProductCatalog>
{
}