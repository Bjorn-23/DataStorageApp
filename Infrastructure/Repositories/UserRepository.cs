using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;


public class UserRepository(DataContext context) : BaseRepository<UserEntity, DataContext>(context), IUserRepository
{
    private readonly DataContext _context = context;

    public override IEnumerable<UserEntity> GetAllWithPredicate(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var existingUserDetails = _context.Users
                .Where(predicate)
                .Include(i => i.UserRole)
                .ToList();

            if (existingUserDetails != null)
            {
                return existingUserDetails;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public override UserEntity GetOne(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var existingUserDetails = _context.Users
                .Include(i => i.UserRole)
                .FirstOrDefault(predicate);
            if (existingUserDetails != null)
            {
                return existingUserDetails;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}

