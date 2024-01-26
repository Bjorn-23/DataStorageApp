using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IOrderRowRepository : IBaseRepository<OrderRowEntity, ProductContext>
{
}