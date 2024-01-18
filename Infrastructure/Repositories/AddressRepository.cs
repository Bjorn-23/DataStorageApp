using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class AddressRepository(DataContext context) : BaseRepository<AddressEntity, DataContext>(context), IAddressRepository
{
    private readonly DataContext _context = context;

}
