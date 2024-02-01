using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductRepository(ProductCatalog context) : BaseRepository<ProductEntity, ProductCatalog>(context), IProductRepository
{
    private readonly ProductCatalog _context = context;

    public override IEnumerable<ProductEntity> GetAll()
    {
        try
        {
            var existingProducts = _context.Products
                .Include(i => i.Price)
                .Include(i => i.CategoryNameNavigation)
                .ToList();

            if (existingProducts.Any())
            {
                return existingProducts;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public override ProductEntity GetOne(Expression<Func<ProductEntity, bool>> predicate)
    {
        try
        {
            var existingProduct = _context.Products
                .Where(predicate)
                .Include(i => i.CategoryNameNavigation)
                .Include(i => i.Price)
                .FirstOrDefault();

            if (existingProduct != null)
            {
                return existingProduct;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}

