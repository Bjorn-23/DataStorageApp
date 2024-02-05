using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class UserRoleService_Tests
{
    private readonly UserRoleService _userRoleService;
    private readonly UserRoleRepository _userRoleRepository;

    public UserRoleService_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _userRoleRepository = new UserRoleRepository(context);
        _userRoleService = new UserRoleService(_userRoleRepository);
    }

    public void GetOrCreateRole_ShouldGetOrCreateARole_AndReturnAUserEntity()
    {
        //Assert
        string roleName = "Admin";
        
        //Act
        var role = _userRoleService.GetOrCreateRole(roleName);

        //Assert
        Assert.NotNull(role);
    }
}
