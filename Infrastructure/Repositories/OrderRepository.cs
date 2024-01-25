using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class OrderRepository(ProductContext context) : BaseRepository<OrderEntity, ProductContext>(context), IOrderRepository
{
    private readonly ProductContext _context = context;
}