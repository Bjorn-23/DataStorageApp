using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CategoryRepository(ProductCatalog context) : BaseRepository<CategoryEntity, ProductCatalog>(context), ICategoryRepository
{
    private readonly ProductCatalog _context = context;

    public override CategoryEntity GetOne(Expression<Func<CategoryEntity, bool>> predicate)
    {
        try
        {
            var existingCategoryDetails = _context.Categories
                .Where(predicate)
                .Include(i => i.Products)
                .FirstOrDefault();

            if (existingCategoryDetails != null)
                return existingCategoryDetails;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}