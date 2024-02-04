using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class UserRoleRepository_Tests : BaseRepository<UserRoleEntity, DataContext>, IUserRoleRepository
{

    private readonly DataContext _context;

    public UserRoleRepository_Tests() : base(new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options))
    {
        _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    }

    [Fact]
    public void CreateShould_CreateOneUserInDatabase_ReturnThatUserIfSuccesfl()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };

        //Act
        var createResult = userRoleRepository.Create(userRole);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Admin", createResult.RoleName);
    }

    [Fact]
    public void CreateShould_NotCreateOneUserInDatabase_ReturnNulll()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() {  };

        //Act
        var createResult = userRoleRepository.Create(userRole);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllUsersFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        //Act
        var getResult = userRoleRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Admin", getResult.FirstOrDefault()!.RoleName);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoUsersInDatabase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var getResult = userRoleRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyUserWithTheSuppliedRoleExists_ReturnThatUserFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        //Act
        var getResult = userRoleRepository.GetAllWithPredicate(x => x.RoleName == createResult.RoleName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Admin", getResult.FirstOrDefault()!.RoleName);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoUserWithTheSuppliedRoleExists_ReturnEmptyList()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);
        string otherRole = "User";


        //Act
        var getResult = userRoleRepository.GetAllWithPredicate(x => x.RoleName == otherRole);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfUserExists_ReturnOneUserFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        //Act
        var getResult = userRoleRepository.GetOne(x => x.RoleName == createResult.RoleName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Admin", getResult.RoleName);
    }

    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole); // No UserRole created, this should fail test.

        //Act
        var getResult = userRoleRepository.GetOne(x => x.RoleName == userRole.RoleName);

        //Assert
        Assert.Null(getResult);
    }

    [Fact] // CANT CHANGE SINCE RoleName is primary Key
    public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = createResult.Id, RoleName = "User" };        
        
        //Act
        var updatedResult = userRoleRepository.Update(createResult, updatedUserRole);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("User", updatedResult.RoleName);
        Assert.NotEqual("Admin", updatedResult.RoleName);
    }

    [Fact]
    public void UpdateShould_FailToUpdateExistingUser_ReturnNull()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = 0, RoleName = "User" };  //changes Id which should make it fail.
        
        //Act
        var updatedResult = userRoleRepository.Update(createResult, updatedUserRole);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]  // CANT CHANGE SINCE RoleName is primary Key
    public void UpdateWithPredicate_Should_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = createResult.Id, RoleName = "User" };  
        
        //Act
        var updatedResult = userRoleRepository.Update(x => x.Id == createResult.Id, updatedUserRole);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("User", updatedResult.RoleName);
        Assert.NotEqual("Admin", updatedResult.RoleName);
    }

    [Fact]
    public void UpdateWithPredicate_Should_NotUpdateExistingUser_ReturnNull()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = 0, RoleName = "User" };  
        
        //Act
        var updatedResult = userRoleRepository.Update(x => x.Id == createResult.Id, updatedUserRole);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = userRoleRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = userRoleRepository.Delete(x => x.Id == userRole.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = userRoleRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = userRoleRepository.Delete(userRole);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = userRoleRepository.Create(userRole);


        //Act
        var updatedResult = userRoleRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        // Arrange
        var userRoleRepository = new UserRoleRepository_Tests();
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);


        //Act
        var updatedResult = userRoleRepository.Exists(x => x.Id == userRole.Id);

        //Assert
        Assert.False(updatedResult);
    }
}
