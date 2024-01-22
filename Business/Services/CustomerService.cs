using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService
{
    private readonly ICustomer_AddressRepository _customer_AddressRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public CustomerService(ICustomerRepository customerRepository, UserService userService, ICustomer_AddressRepository customerAddressRepository, IUserRepository userRepository)
    {
        _customer_AddressRepository = customerAddressRepository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _userService = userService;
    }

    public CustomerDto CreateCustomer(CustomerDto customer)
    {
        try
        {
            var userExists = _userRepository.Exists(x => x.Email == customer.EmailId);
            if (userExists)
            {
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

    public CustomerDetailsDto GetCustomerDetails(CustomerDto customer)
    {
        try
        {
            var existingCustomer = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
            if (existingCustomer != null)
            {
                var userRole = _userService.GetOne(existingCustomer);

                var customerDetails = CustomerFactory.CreateCustomerDetails(existingCustomer, userRole);

                return customerDetails;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<CustomerDto> GetCustomersWithAddressId(AddressDto address)
    {
        try
        {
            var allCustomerAtAddress = _customer_AddressRepository.GetAllWithPredicate(x => x.AddressId == address.Id);

            if (allCustomerAtAddress.Any())
            {
                List<CustomerDto> customerDtos = new();

                foreach (var customer in allCustomerAtAddress)
                {
                    var existingCustomer = _customerRepository.GetOne(x => x.Id == customer.CustomerId);

                    var customerDto = new CustomerDto()
                    {
                        FirstName = existingCustomer.FirstName,
                        LastName = existingCustomer.LastName,
                        EmailId = existingCustomer.EmailId,
                        PhoneNumber = existingCustomer.PhoneNumber,
                    };

                    customerDtos.Add(customerDto);
                }

                return customerDtos;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

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

        return null!;
    }

    
    //------- CURRENTLY NOT USED --------
    public bool CustomerExists(CustomerDto customer)
    {
        try
        {
            CustomerEntity entity = new()
            {

                EmailId = customer.EmailId
            };
            
            var exists = _customerRepository.Exists(x => x.EmailId == entity.EmailId);
            if (exists)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return false;
    }

    public CustomerEntity GetOneCustomer(CustomerDto customer)
    {
        try
        {
            var existingCustomer = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
            if (existingCustomer != null)
            {
                return existingCustomer;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return null!;
    }
}