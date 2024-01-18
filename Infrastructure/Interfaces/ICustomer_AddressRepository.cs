using Infrastructure.Contexts;
using Infrastructure.Entities;


namespace Infrastructure.Interfaces;

public interface ICustomer_AddressRepository : IBaseRepository<Customer_AddressEntity, DataContext>
{
}