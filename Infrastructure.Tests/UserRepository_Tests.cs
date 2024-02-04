using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

//public class UserRepository_Tests(DataContext context) : BaseRepository<UserEntity, DataContext>(context), IUserRepository
public class UserRepository_Tests : BaseRepository<UserEntity, DataContext>, IUserRepository
{

    private readonly DataContext _context;

    public UserRepository_Tests() : base(new DataContext(new DbContextOptionsBuilder<DataContext>()
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
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };

        //Act
        var createResult = userRepository.Create(user);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("bjorn@domain.com", createResult.Email);
    }

    [Fact]
    public void CreateShould_NotCreateOneUserInDatabase_ReturnNulll()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 }; // Email left out: Email = "bjorn@domain.com",

        //Act
        var createResult = userRepository.Create(user);

        //Assert
        Assert.Null(createResult);
    }
    
    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllUsersFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        //Act
        var getResult = userRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.FirstOrDefault()!.Email);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoUsersInDatabase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();

        //Act
        var getResult = userRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyUserWithTheSuppliedRoleExists_ReturnThatUserFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        //Act
        var getResult = userRepository.GetAllWithPredicate(x => x.UserRoleId == createResult.UserRoleId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal(1, getResult.FirstOrDefault()!.UserRoleId);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoUserWithTheSuppliedRoleExists_ReturnEmptyList()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        int otherRoleId = 2;
        var createResult = userRepository.Create(user);

        //Act
        var getResult = userRepository.GetAllWithPredicate(x => x.UserRoleId == otherRoleId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfUserExists_ReturnOneUserFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        //Act
        var getResult = userRepository.GetOne(x => x.Email == createResult.Email);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.Email);
    }

    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        //var createResult = userRepository.Create(user); // User is never created in this test

        //Act
        var getResult = userRepository.GetOne(x => x.Email == user.Email);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        var updatedUser = new UserEntity() { Id = user.Id, Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = userRepository.Update(createResult, updatedUser);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.Email);
        Assert.NotEqual(1 ,updatedResult.UserRoleId);
    }

    [Fact]
    public void UpdateShould_FailToUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        var updatedUser = new UserEntity() { /*Id = user.Id,*/ Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = userRepository.Update(createResult, updatedUser);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_Should_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        var updatedUser = new UserEntity() { Id = user.Id, Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = userRepository.Update(x => x.Id == createResult.Id, updatedUser);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.Email);
        Assert.NotEqual(1, updatedResult.UserRoleId);
    }

    [Fact]
    public void UpdateWithPredicate_Should_NotUpdateExistingUser_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);

        var updatedUser = new UserEntity() { /*Id = user.Id,*/ Password = user.Password, SecurityKey = user.SecurityKey, IsActive = user.IsActive, Created = user.Created, Email = "bjorn@email.com", UserRoleId = 2 };
        //Act
        var updatedResult = userRepository.Update(x => x.Id == createResult.Id, updatedUser);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);


        //Act
        var updatedResult = userRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        //var createResult = userRepository.Create(user);


        //Act
        var updatedResult = userRepository.Delete(x => x.Id == user.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);


        //Act
        var updatedResult = userRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        //var createResult = userRepository.Create(user);


        //Act
        var updatedResult = userRepository.Delete(user);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1 };
        var createResult = userRepository.Create(user);


        //Act
        var updatedResult = userRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        //Arrange
        var userRepository = new UserRepository_Tests();
        var user = new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = 1   };
        //var createResult = userRepository.Create(user);


        //Act
        var updatedResult = userRepository.Exists(x => x.Id == user.Id);

        //Assert
        Assert.False(updatedResult);
    }

}
