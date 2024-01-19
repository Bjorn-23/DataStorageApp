using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity, DataContext>
{
}