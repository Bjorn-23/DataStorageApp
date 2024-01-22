using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;
using System.Net.Http.Headers;

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

        return new List<CustomerDto>();
    }

    public CustomerDto UpdateCustomer(CustomerEntity existingCustomer, CustomerDto newDetails)
    {
        try
        {
            var newEntity = CustomerFactory.Create(newDetails);

            
            var result = _customerRepository.Update(x => x.EmailId == existingCustomer.EmailId, newEntity);
            if (result != null)
            {
               var customer = CustomerFactory.Create(result);
                return customer;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public bool DeleteCustomer(CustomerDto customer, string option)
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
                var result =_customerRepository.Delete(entity);
                if (result)
                    return true;
            }


        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
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

    public CustomerDto GetOneCustomer(CustomerDto customer, string option)
    {
        try
        {
            CustomerEntity existingCustomer = new();

            switch (option)
            {
                case "1":
                    existingCustomer = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
                    break;
                case "2":
                    existingCustomer = _customerRepository.GetOne(x => x.Id == customer.Id);
                    break;
                case "3":
                    existingCustomer = _customerRepository.GetOne(x => x.PhoneNumber == customer.PhoneNumber);
                    break;
            }

            if (existingCustomer != null)
            {
                var customerDto = CustomerFactory.Create(existingCustomer); 
                return customerDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!; ;
    }
}