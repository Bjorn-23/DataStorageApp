using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class CustomerRepository(DataContext context) : BaseRepository<CustomerEntity, DataContext>(context), ICustomerRepository
{
    private readonly DataContext _context = context;
}

