using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity, DataContext>(context), IUserRepository
{
    private readonly DataContext _context = context;
}
