using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net;

namespace Infrastructure.Tests.Repository_Tests;

public class CustomerRepository_Tests
{
    private readonly CustomerRepository _customerRepository;
    private readonly UserRepository _userRepository;
    private readonly Customer_AddressRepository _customerAddressRepository;
    private readonly UserRoleRepository _userRoleRepository;
    private readonly AddressRepository _addressRepository;

    public CustomerRepository_Tests()
    {
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

        _customerRepository = new CustomerRepository(context);
        _userRepository = new UserRepository(context);
        _customerAddressRepository = new Customer_AddressRepository(context);
        _userRoleRepository = new UserRoleRepository(context);
        _addressRepository = new AddressRepository(context);
    }

    [Fact]
    public void Create_ShouldCreateNewEntityInDatabase_AndReturnIt()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };

        //Act
        var createResult = _customerRepository.Create(customer);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("bjorn@domain.com", createResult.EmailId);
    }

    [Fact]
    public void Create_ShouldNotCreateNewEntityInDatabase_AndReturnNull()
    {
        //Arrange
        var customer = new CustomerEntity() { FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" }; // Email left out: EmailId = "bjorn@domain.com",

        //Act
        var createResult = _customerRepository.Create(customer);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyEntityExists_ReturnAllEntitiesFromDataBase()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        //Act
        var getResult = _customerRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.FirstOrDefault()!.EmailId);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_SinceNoEntitiesExistInDatabase()
    {
        //Arrange


        //Act
        var getResult = _customerRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnAnyUser_MatchingLambdaExpressionFromDataBase()
    {
        //Arrange
        var UserRole = _userRoleRepository.Create(new UserRoleEntity() { RoleName = "Demo Role" });
        var user = _userRepository.Create(new UserEntity() { Email = "bjorn@domain.com", Password = "Bytmig123!", SecurityKey = "blabla", IsActive = true, UserRoleId = UserRole.Id });
        var customer = new CustomerEntity() { EmailId = user.Email, FirstName = "Björn", LastName = "Andersson", Id = user.Id, PhoneNumber = "0789456123" };
        var address = _addressRepository.Create(new AddressEntity() { StreetName = "Storgatan 1", PostalCode = "222 22", City = "Storstan", Country = "Sverige" });
        var customer_address = _customerAddressRepository.Create(new Customer_AddressEntity() { AddressId = address.Id, CustomerId = customer.Id });
        var createResult = _customerRepository.Create(customer);

        //Act
        var getResult = _customerRepository.GetAllWithPredicate(x => x.FirstName == createResult.FirstName);

        //Assert
        Assert.NotNull(getResult);
        //Assert.Equal("Björn", getResult.FirstOrDefault()!.FirstName);
    }

    [Fact]
    public void GetAllWithPredicate_ShouldReturnEmptyList_SinceTheEntityInPredicate_DoesNotExistInDatabase()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        string otherRole = "Arne";
        var createResult = _customerRepository.Create(customer);

        //Act
        var getResult = _customerRepository.GetAllWithPredicate(x => x.FirstName == otherRole);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOne_ShouldIfEntityExists_ReturnOneEntityFromDataBase()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        //Act
        var getResult = _customerRepository.GetOne(x => x.EmailId == createResult.EmailId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.EmailId);
    }

    [Fact]
    public void GetOne_ShouldReturnNull_SinceNoEntitiesExists()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = userRepository.Create(user); // User is never created in this test

        //Act
        var getResult = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void Update_ShouldUpdateExistingEntity_ReturnUpdatedEntity_FromDataBase()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, PhoneNumber = "01253456789", EmailId = "bjorn@email.com" }; // Different email

        //Act
        var updatedResult = _customerRepository.Update(createResult, updatedCustomer);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.EmailId);
        Assert.NotEqual("0789456123", updatedResult.PhoneNumber);
    }

    [Fact]
    public void Update_ShouldFailToUpdateExistingEntity_AndReturnNull()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" }; // No Id supplied

        //Act
        var updatedResult = _customerRepository.Update(createResult, updatedCustomer);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldUpdateExistingEntity_AndReturnUpdatedEntityFromDataBase()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, PhoneNumber = "01253456789", EmailId = "bjorn@email.com" };
        //Act
        var updatedResult = _customerRepository.Update(x => x.Id == createResult.Id, updatedCustomer);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.EmailId);
        Assert.NotEqual("0789456123", updatedResult.PhoneNumber);
    }

    [Fact]
    public void UpdateWithPredicate_ShouldNotUpdateExistingEntity_AndThenReturnNull()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { EmailId = "bjorn@email.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "01253456789" };

        //Act
        var updatedResult = _customerRepository.Update(x => x.Id == createResult.Id, updatedCustomer);


        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);

        //Act
        var updatedResult = _customerRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = _customerRepository.Create(customer);

        //Act
        var updatedResult = _customerRepository.Delete(x => x.Id == customer.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Delete_ShouldDeleteExistingEntity_AndReturnTrue()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);


        //Act
        var updatedResult = _customerRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_ShouldNotDeleteAnyEntities_AndReturnFalse()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = _customerRepository.Create(customer);


        //Act
        var updatedResult = _customerRepository.Delete(customer);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnTrue()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = _customerRepository.Create(customer);


        //Act
        var updatedResult = _customerRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void Exists_ShouldCheckForExistingEntity_AndReturnFalse()
    {
        //Arrange
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = _customerRepository.Create(customer);


        //Act
        var updatedResult = _customerRepository.Exists(x => x.Id == customer.Id);

        //Assert
        Assert.False(updatedResult);
    }
}

