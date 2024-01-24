using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Business.Utils;

public static class RoleCheck
{
    //private readonly UserRepository _userRepository;

    //public RoleCheck(UserRepository userRepository)
    //{
    //    _userRepository = userRepository;
    //}

    //public static UserEntity FindRoleOfActiveUser() //Inefficient in large databases, but works for now as a logout button that doesnt require user input.
    //{
    //    try
    //    {
    //        var userEntities = _userRepository.GetAllWithPredicate(x => x.isActive == true);

    //        foreach (var user in userEntities)
    //        {
    //            return user;
    //        }
    //    }
    //    catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

    //    return null!;
    //}

}
