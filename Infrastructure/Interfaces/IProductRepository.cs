using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IBaseRepository<ProductEntity, ProductContext>
{
}