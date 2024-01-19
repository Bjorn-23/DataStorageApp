using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class UserRoleService(IUserRoleRepository userRoleRepository)
{
    private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;
}
