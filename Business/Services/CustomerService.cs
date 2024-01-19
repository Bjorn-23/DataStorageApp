using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public bool CustomerExists(CustomerDto customer)
    {
        try
        {
            CustomerEntity entity = new()
            {

                EmailId = customer.EmailId
            };
            
            var exists = _customerRepository.Exists(x => x.Email == entity.Email);
            if (exists)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return false;
    }

    public CustomerDto CreateCustomer(CustomerDto customer)
    {
        try
        {
            CustomerEntity entity = new()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailId = customer.EmailId,
                //Password = customer.Password, // should be in UserDto
                PhoneNumber = customer.PhoneNumber,
            };
                
            var result = _customerRepository.Create(entity);
            if (result != null)
            {
                return customer;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return null!;
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

    public IEnumerable<CustomerDto> GetAll()
    {
        try
        {
            var allCustomers = _customerRepository.GetAll();
            if (allCustomers != null)
            {
                List<CustomerDto> customerList = new();

                foreach (var customer in allCustomers)
                {
                    CustomerDto customerDto = new()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        EmailId = customer.EmailId,
                        PhoneNumber = customer.PhoneNumber
                    };

                    customerList.Add(customerDto);
                }

                return customerList;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}


        //try
        //{

        //}
        //catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }