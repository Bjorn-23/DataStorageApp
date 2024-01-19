using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UserRoleRepository(DataContext context) : BaseRepository<UserRoleEntity, DataContext>(context), IUserRoleRepository
{
    private readonly DataContext _context = context;

}
