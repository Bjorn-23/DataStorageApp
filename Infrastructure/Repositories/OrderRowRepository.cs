using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class OrderRowRepository(ProductContext context) : BaseRepository<OrderRowEntity, ProductContext>(context), IOrderRowRepository
{
    private readonly ProductContext context = context;
}

