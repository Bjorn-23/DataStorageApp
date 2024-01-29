using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class OrderRowRepository(ProductCatalog context) : BaseRepository<OrderRowEntity, ProductCatalog>(context), IOrderRowRepository
{
    private readonly ProductCatalog _context = context;

    public override IEnumerable<OrderRowEntity> GetAllWithPredicate(Expression<Func<OrderRowEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = _context.OrderRows
                .Include(i => i.Order)
                .Include(i => i.ArticleNumberNavigation).ThenInclude(i => i.Price)
                .Where(predicate)
                .ToList();

            return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}

