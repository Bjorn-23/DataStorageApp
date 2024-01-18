using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        CustomerEntity entity = new()
        {

            Email = customer.Email
        };
            
        var exists = _customerRepository.Exists(x => x.Email == entity.Email);
        if (exists)
        {
            return true;
        }
        return false;
    }

    public CustomerDto CreateCustomer(CustomerDto customer)
    {
        CustomerEntity entity = new()
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Password = customer.Password,
            PhoneNumber = customer.PhoneNumber,
        };
                
        var result = _customerRepository.Create(entity);
        if (result != null)
        {
            return customer;
        }
        return null!;
    }

    public CustomerEntity GetOneCustomer(CustomerDto customer)
    {
        var existingCustomer = _customerRepository.GetOne(x => x.Email == customer.Email);
        if (existingCustomer != null)
        {
            return existingCustomer;

            // THIS IS WHERE WE NEED A FACTORY

            //CustomerDto customerDto = new()
            //{
            //    FirstName = existingCustomer.FirstName,
            //    LastName = existingCustomer.LastName,
            //    Email = existingCustomer.Email,
            //    PhoneNumber = existingCustomer.PhoneNumber,
            //};
            //return customerDto;
        }
        return null!;
    }






}
