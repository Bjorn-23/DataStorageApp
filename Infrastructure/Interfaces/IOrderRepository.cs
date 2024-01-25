using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IOrderRepository : IBaseRepository<OrderEntity, ProductContext>
{
}