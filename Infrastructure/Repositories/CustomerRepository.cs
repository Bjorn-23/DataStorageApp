using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CustomerRepository(DataContext context) : BaseRepository<CustomerEntity, DataContext>(context), ICustomerRepository
{
    private readonly DataContext _context = context;

    public override IEnumerable<CustomerEntity> GetAllWithPredicate(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var customerDetailsEntity = _context.Customers
                .Include(i => i.User).ThenInclude(i => i.UserRole)
                .Include(i => i.CustomerAddresses).ThenInclude(i => i.Address)
                .Where(predicate)
                .ToList();

            return customerDetailsEntity;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}

