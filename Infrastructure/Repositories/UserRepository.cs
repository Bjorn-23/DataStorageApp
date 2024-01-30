using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity, DataContext>(context), IUserRepository
{
    private readonly DataContext _context = context;

    public override UserEntity GetOne(Expression<Func<UserEntity, bool>> predicate)
    {
        return base.GetOne(predicate);
    }
}
