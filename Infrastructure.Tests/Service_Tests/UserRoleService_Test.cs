using Business.Dtos;
using Business.Factories;
using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Service_Tests;

public class UserRoleService_Test
{
    private readonly UserRoleService _userRoleService;
    private readonly UserRoleRepository _userRoleRepository;

    public UserRoleService_Test()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}")
            .Options);

        _userRoleRepository = new UserRoleRepository(context);
        _userRoleService = new UserRoleService(_userRoleRepository);
    }

    [Fact]
    public void GetOrCreateRole_ShouldGetOrCreateARole_AndReturnAUserEntity()
    {
        //Arrange        
        string roleName = "Admin";

        //Act
        var createRole = _userRoleService.GetOrCreateRole(roleName);
        var GetRole = _userRoleService.GetOrCreateRole(roleName);


        //Assert
        Assert.NotNull(createRole);
        Assert.NotNull(GetRole);
        Assert.Equal(createRole.UserRoleId, GetRole.UserRoleId);
    }

    [Fact]
    public void GetAll_ShouldGetAllUserRolesFromDatabase_AndReturnAsAList()
    {
        //Arrange
        string roleName = "Admin";
        var createRole = _userRoleService.GetOrCreateRole(roleName);

        //Act
        var result = _userRoleService.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.FirstOrDefault()!.RoleName);
    }

    [Fact]
    public void UpdateRole_ShouldUpdateExistingRoleNameWithNewRoleName_AndReturnUserRoleDto()
    {
        //Arrange
        var createRole = _userRoleService.GetOrCreateRole("Admin");
        UserRoleDto UpdatedRole = new() { Id = createRole.UserRoleId, RoleName = "User" };
        var existingUserRole = UserRoleFactory.Create(_userRoleRepository.GetOne(x => x.RoleName == "Admin"));

        //Act
        var updatedUserRole = _userRoleService.UpdateRole(existingUserRole, UpdatedRole);

        //Assert
        Assert.NotNull(updatedUserRole);
        Assert.Equal("User", updatedUserRole.RoleName);
    }

    [Fact]
    public void DeleteRole_ShouldDeleteRoleEntityFromDatabase_AndReturnDeletedRoleDto()
    {
        //Arrange
        var createRole = _userRoleService.GetOrCreateRole("Admin");
        var existingUserRole = UserRoleFactory.Create(_userRoleRepository.GetOne(x => x.RoleName == "Admin"));

        //Act
        var deletedUserRole = _userRoleService.DeleteRole(existingUserRole);
        
        //Assert
        Assert.NotNull(deletedUserRole);
        Assert.Equal(existingUserRole.RoleName, deletedUserRole.RoleName);
    }
}
