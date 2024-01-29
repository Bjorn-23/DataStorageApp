using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class OrderRepository(ProductCatalog context) : BaseRepository<OrderEntity, ProductCatalog>(context), IOrderRepository
{
    private readonly ProductCatalog _context = context;
}