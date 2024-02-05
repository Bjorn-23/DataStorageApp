using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class UserRoleRepository_Tests
{

    private readonly UserRoleRepository _userRoleRepository;
    public UserRoleRepository_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _userRoleRepository = new UserRoleRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };

        //Act
        var createResult = _userRoleRepository.Create(userRole);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("Admin", createResult.RoleName);
    }

    [Fact]
    public void CreateShould_NotCreateOneUCreate_ShouldNotCreateNewEntityInDatabase_AndReturnNullserInDatabase_ReturnNulll()
    {
        //Arrange
        var userRole = new UserRoleEntity() {  };

        //Act
        var createResult = _userRoleRepository.Create(userRole);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        //Act
        var getResult = _userRoleRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Admin", getResult.FirstOrDefault()!.RoleName);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var getResult = _userRoleRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        //Act
        var getResult = _userRoleRepository.GetAllWithPredicate(x => x.RoleName == createResult.RoleName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Admin", getResult.FirstOrDefault()!.RoleName);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);
        string otherRole = "User";


        //Act
        var getResult = _userRoleRepository.GetAllWithPredicate(x => x.RoleName == otherRole);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        //Act
        var getResult = _userRoleRepository.GetOne(x => x.RoleName == createResult.RoleName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Admin", getResult.RoleName);
    }

    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole); // No UserRole created, this should fail test.

        //Act
        var getResult = _userRoleRepository.GetOne(x => x.RoleName == userRole.RoleName);

        //Assert
        Assert.Null(getResult);
    }

    [Fact] // CANT CHANGE SINCE RoleName is primary Key
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = createResult.Id, RoleName = "User" };        
        
        //Act
        var updatedResult = _userRoleRepository.Update(createResult, updatedUserRole);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("User", updatedResult.RoleName);
        Assert.NotEqual("Admin", updatedResult.RoleName);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = 0, RoleName = "User" };  //changes Id which should make it fail.
        
        //Act
        var updatedResult = _userRoleRepository.Update(createResult, updatedUserRole);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]  // CANT CHANGE SINCE RoleName is primary Key
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = createResult.Id, RoleName = "User" };  
        
        //Act
        var updatedResult = _userRoleRepository.Update(x => x.Id == createResult.Id, updatedUserRole);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("User", updatedResult.RoleName);
        Assert.NotEqual("Admin", updatedResult.RoleName);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        var updatedUserRole = new UserRoleEntity() { Id = 0, RoleName = "User" };  
        
        //Act
        var updatedResult = _userRoleRepository.Update(x => x.Id == createResult.Id, updatedUserRole);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        //Act
        var updatedResult = _userRoleRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = _userRoleRepository.Delete(x => x.Id == userRole.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        //Act
        var updatedResult = _userRoleRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = _userRoleRepository.Delete(userRole);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        var createResult = _userRoleRepository.Create(userRole);

        //Act
        var updatedResult = _userRoleRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var userRole = new UserRoleEntity() { RoleName = "Admin" };
        //var createResult = userRoleRepository.Create(userRole);

        //Act
        var updatedResult = _userRoleRepository.Exists(x => x.Id == userRole.Id);

        //Assert
        Assert.False(updatedResult);
    }
}
