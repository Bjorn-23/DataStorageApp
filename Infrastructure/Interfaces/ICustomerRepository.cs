using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository : IBaseRepository<CustomerEntity, DataContext>
{
}