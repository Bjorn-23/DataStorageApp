using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class OrderRepository(ProductCatalog context) : BaseRepository<OrderEntity, ProductCatalog>(context), IOrderRepository
{
    private readonly ProductCatalog _context = context;

    public override IEnumerable<OrderEntity> GetAllWithPredicate(Expression<Func<OrderEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = _context
                .Set<OrderEntity>()
                .Include(i => i.OrderRows).ThenInclude(i => i.ArticleNumberNavigation).ThenInclude(i => i.Price)
                .Where(predicate)
                .ToList();

            return existingEntity;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}