using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Business.Services;

public class Customer_AddressService
{
    private readonly ICustomer_AddressRepository _customer_AddressRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ICustomerRepository _customerRepository;

    public Customer_AddressService(ICustomer_AddressRepository customer_AddressRepository, IAddressRepository addressRepository, ICustomerRepository customerRepository)
    {
        _customer_AddressRepository = customer_AddressRepository;
        _addressRepository = addressRepository;
        _customerRepository = customerRepository;
    }

    public bool CreateCustomer_Addresses(CustomerDto customer, AddressDto address)
    {
        var customerExists = _customerRepository.GetOne(x => x.Email == customer.Email);

        var addressExists = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);

        if (customerExists != null && addressExists != null)
        {
            Customer_AddressEntity entity = new Customer_AddressEntity()
            {
                CustomerId = customerExists.Id,
                AddressId = addressExists.Id,
            };

            var customer_AddressExists = _customer_AddressRepository.Exists(x => x.AddressId == addressExists.Id && x.CustomerId == customerExists.Id);
            
            if (!customer_AddressExists)
            {
                var newCustomer_Address = _customer_AddressRepository.Create(entity);
                if (newCustomer_Address != null)
                {
                    return true;
                }
            }
            return true;

        }
        return false;
    }

}
