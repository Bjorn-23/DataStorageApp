using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class Customer_AddressRepository(DataContext context) : BaseRepository<Customer_AddressEntity, DataContext>(context), ICustomer_AddressRepository
{
    private readonly DataContext _context = context;
}
