using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;

    public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Creates new customer provided a user exists with the same Email. 
    /// </summary>
    /// <param name="customer"></param>
    /// <returns>CustomerDto</returns>
    public CustomerDto CreateCustomer(CustomerDto customer)
    {
        try
        {
            var existingUser = _userRepository.GetOne(x => x.Email == customer.EmailId);
            if (existingUser != null)
            {
                customer.Id = existingUser.Id;
                var entity = CustomerFactory.Create(customer);
                
                var result = _customerRepository.Create(entity);
                if (result != null)
                {
                    return customer;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return null!;
    }

    /// <summary>
    /// Fetches one customer from database along with associated addreses and user Role information.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns>List of AddressDto, UserRoleDto and CustomerDto</returns>
    public (IEnumerable<AddressDto> address, UserRoleDto userRole, CustomerDto customer) GetOneCustomerWithDetails(CustomerDto customer)
    {
        try
        {
            var existingCustomerDetails = _customerRepository.GetAllWithPredicate(x => x.EmailId == customer.EmailId);
            if (existingCustomerDetails.Any())
            {
                List<AddressDto> addressDtos = new();

                foreach (var existingCustomer in existingCustomerDetails)
                {
                    var customerDto = CustomerFactory.Create(existingCustomer);
                    var userRoleDto = UserRoleFactory.Create(existingCustomer.User);

                    foreach (var addresses in existingCustomer.CustomerAddresses)
                    {
                        var addressDto = AddressFactory.Create(addresses.Address);       
                        addressDtos.Add(addressDto);
                    }

                    return (addressDtos, userRoleDto, customerDto);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return (null!, null!, null!);
    }

    /// <summary>
    /// GetOneCustomer, checks if customer exists in database and returns it.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns>CustomerDto</returns>
    public CustomerDto GetOneCustomer(CustomerDto customer)
    {
        try
        {
            var existingEntity = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
            if (existingEntity != null)
            {
                var existingCustomer = CustomerFactory.Create(existingEntity);
                return existingCustomer;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    /// <summary>
    /// Fetches all Customers from database and returns them.
    /// </summary>
    /// <returns>List of CustomerDto</returns>
    public IEnumerable<CustomerDto> GetAll()
    {
        try
        {
            var allCustomers = _customerRepository.GetAll();
            if (allCustomers != null)
            {
                var customerList = CustomerFactory.Create(allCustomers);
                return customerList;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<CustomerDto>();
    }

    /// <summary>
    /// Updates existing customer in database.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="newCustomerDetails"></param>
    /// <returns>CustomerDto</returns>
    public CustomerDto UpdateCustomer(CustomerDto customer, CustomerDto newCustomerDetails)
    {
        try
        {
            var existingCustomer = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
            if (existingCustomer != null)
            {        
                CustomerEntity newCustomerEntity = new()
                {
                    Id = existingCustomer.Id,
                    FirstName = string.IsNullOrWhiteSpace(newCustomerDetails.FirstName) ? existingCustomer.FirstName : newCustomerDetails.FirstName,
                    LastName = string.IsNullOrWhiteSpace(newCustomerDetails.LastName) ? existingCustomer.LastName : newCustomerDetails.LastName,
                    EmailId = string.IsNullOrWhiteSpace(newCustomerDetails.EmailId) ? existingCustomer.EmailId : newCustomerDetails.EmailId,
                    PhoneNumber = string.IsNullOrWhiteSpace(newCustomerDetails.PhoneNumber) ? existingCustomer.PhoneNumber : newCustomerDetails.PhoneNumber
                };

                var result = _customerRepository.Update(x => x.EmailId == existingCustomer.EmailId, newCustomerEntity);
                if (result != null)
                {
                    var updatedCustomer = CustomerFactory.Create(result);
                    return updatedCustomer;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
    
    /// <summary>
    /// Deletes existing Customer from database.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="option"></param>
    /// <returns>CustomerDto</returns>
    public CustomerDto DeleteCustomer(CustomerDto customer, string option) // Currently undecided if I should let Users delete customer.
    {
        try
        {
            CustomerEntity entity = new();

            switch (option)
            {
                case "1":
                    entity = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
                    break;
                case "2":
                    entity = _customerRepository.GetOne(x => x.Id == customer.Id);
                    break;
                case "3":
                    entity = _customerRepository.GetOne(x => x.PhoneNumber == customer.PhoneNumber);
                    break;
            }

            if (entity != null)
            {
                var result = _customerRepository.Delete(entity);
                if (result)
                    return CustomerFactory.Create(entity);
            }


        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}