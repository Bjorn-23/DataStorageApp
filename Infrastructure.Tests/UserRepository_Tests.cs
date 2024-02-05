using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Tests;

public class UserRepository_Tests
{

    private readonly UserRepository _userRepository;
    private readonly UserRoleRepository _roleRepository;

    public UserRepository_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _userRepository = new UserRepository(context);
        _roleRepository = new UserRoleRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };

        //Act
        var createResult = _userRepository.Create(user);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("bjorn@domain.com", createResult.Email);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var user = new UserEntity() { Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 }; // Email left out: Email = "bjorn@domain.com",

        //Act
        var createResult = _userRepository.Create(user);

        //Assert
        Assert.Null(createResult);
    }
    
    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        //Act
        var getResult = _userRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.FirstOrDefault()!.Email);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange

        //Act
        var getResult = _userRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var userRole = _roleRepository.Create(new UserRoleEntity() { Id = 1, RoleName = "Admin" });
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        //Act
        var getResult = _userRepository.GetAllWithPredicate(x => x.Email == createResult.Email);

        //Assert
        Assert.NotNull(getResult);
        Assert.Contains(createResult, getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var userRole = _roleRepository.Create(new UserRoleEntity() { Id = 1, RoleName = "Admin" });
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        int otherRoleId = 2;
        var createResult = _userRepository.Create(user);

        //Act
        var getResult = _userRepository.GetAllWithPredicate(x => x.UserRoleId == otherRoleId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var userRole = _roleRepository.Create(new UserRoleEntity() { Id = 1, RoleName = "Admin" });
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        //Act
        var getResult = _userRepository.GetOne(x => x.Email == createResult.Email);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.Email);
    }

    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var userRole = _roleRepository.Create(new UserRoleEntity() { Id = 1, RoleName = "Admin" });
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        //var createResult = _userRepository.Create(user); // User is never created in this test

        //Act
        var getResult = _userRepository.GetOne(x => x.Email == user.Email);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        var updatedUser = new UserEntity() { Id = user.Id, Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = _userRepository.Update(createResult, updatedUser);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.Email);
        Assert.NotEqual(1 ,updatedResult.UserRoleId);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        var updatedUser = new UserEntity() { /*Id = user.Id,*/ Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = _userRepository.Update(createResult, updatedUser);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        var updatedUser = new UserEntity() { Id = user.Id, Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = _userRepository.Update(x => x.Id == createResult.Id, updatedUser);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.Email);
        Assert.NotEqual(1, updatedResult.UserRoleId);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);

        var updatedUser = new UserEntity() { /*Id = user.Id,*/ Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = _userRepository.Update(x => x.Id == createResult.Id, updatedUser);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);


        //Act
        var updatedResult = _userRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        //var createResult = _userRepository.Create(user);


        //Act
        var updatedResult = _userRepository.Delete(x => x.Id == user.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);


        //Act
        var updatedResult = _userRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        //var createResult = _userRepository.Create(user);


        //Act
        var updatedResult = _userRepository.Delete(user);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = _userRepository.Create(user);


        //Act
        var updatedResult = _userRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1   };
        //var createResult = _userRepository.Create(user);


        //Act
        var updatedResult = _userRepository.Exists(x => x.Id == user.Id);

        //Assert
        Assert.False(updatedResult);
    }

}
