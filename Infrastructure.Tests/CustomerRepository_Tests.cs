using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests;

public class CustomerRepository_Tests : BaseRepository<CustomerEntity, DataContext>, ICustomerRepository
{
    private readonly DataContext _context;

    public CustomerRepository_Tests() : base(new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options))
    {
        _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);
    }

    [Fact]
    public void CreateShould_CreateOneCustomerInDatabase_ReturnThatCustomerIfSuccesful()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };

        //Act
        var createResult = customerRepository.Create(customer);

        //Assert
        Assert.NotNull(createResult);
        Assert.Equal("bjorn@domain.com", createResult.EmailId);
    }

    [Fact]
    public void CreateShould_NotCreateOneCustomerInDatabase_ReturnNulll()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" }; // Email left out: EmailId = "bjorn@domain.com",

        //Act
        var createResult = customerRepository.Create(customer);

        //Assert
        Assert.Null(createResult);
    }

    [Fact]
    public void GetAllShould_IfAnyUserExists_ReturnAllCustomerFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        //Act
        var getResult = customerRepository.GetAll();

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.FirstOrDefault()!.EmailId);
    }

    [Fact]
    public void GetAllShould_ReturnEmptyList_SinceNoCustomerInDatabase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();

        //Act
        var getResult = customerRepository.GetAll();

        //Assert
        Assert.Empty(getResult);
        Assert.NotNull(getResult);
    }

    [Fact]
    public void GetAllWithPredicate_Should_IfAnyCustomerWithTheSuppliedFirstNameExists_ReturnThatCustomerFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        //Act
        var getResult = customerRepository.GetAllWithPredicate(x => x.FirstName == createResult.FirstName);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("Björn", getResult.FirstOrDefault()!.FirstName);
    }

    [Fact]
    public void GetAllWithPredicate_Should_SinceNoCustomerWithTheSuppliedFirstNameExists_ReturnEmptyList()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        string otherRole = "Arne";
        var createResult = customerRepository.Create(customer);

        //Act
        var getResult = customerRepository.GetAllWithPredicate(x => x.FirstName == otherRole);

        //Assert
        Assert.NotNull(getResult);
        Assert.Empty(getResult);
    }

    [Fact]
    public void GetOneShould_IfCustomerExists_ReturnOneCustomerFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        //Act
        var getResult = customerRepository.GetOne(x => x.EmailId == createResult.EmailId);

        //Assert
        Assert.NotNull(getResult);
        Assert.Equal("bjorn@domain.com", getResult.EmailId);
    }

    [Fact]
    public void GetOneShould_SinceNoUserExists_ReturnNull()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = userRepository.Create(user); // User is never created in this test

        //Act
        var getResult = customerRepository.GetOne(x => x.EmailId == customer.EmailId);

        //Assert
        Assert.Null(getResult);
    }

    [Fact]
    public void UpdateShould_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, PhoneNumber = "01253456789", EmailId = "bjorn@email.com"}; // Different email
       
        //Act
        var updatedResult = customerRepository.Update(createResult, updatedCustomer);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.EmailId);
        Assert.NotEqual("0789456123", updatedResult.PhoneNumber);
    }

    [Fact]
    public void UpdateShould_FailToUpdateExistingUser_BecauseOfPrimaryKeyBeingChanged_AndReturnNull()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() {EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123"}; // No Id supplied
       
        //Act
        var updatedResult = customerRepository.Update(createResult, updatedCustomer);

        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void UpdateWithPredicate_Should_UpdateExistingUser_ReturnUpdatedUserFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName, PhoneNumber = "01253456789", EmailId = "bjorn@email.com" };
        //Act
        var updatedResult = customerRepository.Update(x => x.Id == createResult.Id, updatedCustomer);

        //Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("bjorn@email.com", updatedResult.EmailId);
        Assert.NotEqual("0789456123", updatedResult.PhoneNumber);
    }

    [Fact]
    public void UpdateWithPredicate_Should_NotUpdateExistingUser_ReturnNull()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        var updatedCustomer = new CustomerEntity() { EmailId = "bjorn@email.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "01253456789" };

        //Act
        var updatedResult = customerRepository.Update(x => x.Id == createResult.Id, updatedCustomer);


        //Assert
        Assert.Null(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);

        //Act
        var updatedResult = customerRepository.Delete(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteWithPredicate_Should_NotDeleteExistingUser_ReturnFalse()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = customerRepository.Create(customer);

        //Act
        var updatedResult = customerRepository.Delete(x => x.Id == customer.Id);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void DeleteShould_DeleteExistingUser_ReturnDeletedUserFromDataBase()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);


        //Act
        var updatedResult = customerRepository.Delete(createResult);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void DeleteShould_NotDeleteExistingUser_ReturnNull()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = customerRepository.Create(customer);


        //Act
        var updatedResult = customerRepository.Delete(customer);

        //Assert
        Assert.False(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnTrueIfUserExists()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        var createResult = customerRepository.Create(customer);


        //Act
        var updatedResult = customerRepository.Exists(x => x.Id == createResult.Id);

        //Assert
        Assert.True(updatedResult);
    }

    [Fact]
    public void ExistsShould_CheckForExistingUser_ReturnFalseSinceUserDoesntExist()
    {
        // Arrange
        var customerRepository = new CustomerRepository_Tests();
        var customer = new CustomerEntity() { EmailId = "bjorn@domain.com", FirstName = "Björn", LastName = "Andersson", Id = Guid.NewGuid().ToString(), PhoneNumber = "0789456123" };
        //var createResult = customerRepository.Create(customer);


        //Act
        var updatedResult = customerRepository.Exists(x => x.Id == customer.Id);

        //Assert
        Assert.False(updatedResult);
    }
}

