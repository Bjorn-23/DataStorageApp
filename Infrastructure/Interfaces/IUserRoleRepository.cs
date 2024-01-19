using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRoleRepository : IBaseRepository<UserRoleEntity, DataContext>
{
}