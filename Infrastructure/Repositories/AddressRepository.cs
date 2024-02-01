using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class AddressRepository(DataContext context) : BaseRepository<AddressEntity, DataContext>(context), IAddressRepository
{
    private readonly DataContext _context = context;

    public override IEnumerable<AddressEntity> GetAllWithPredicate(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var addressDetailsEntity = _context.Address
                .Include(i => i.CustomerAddresses).ThenInclude(i => i.Customer)
                .Where(predicate)
                .ToList();

            return addressDetailsEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
