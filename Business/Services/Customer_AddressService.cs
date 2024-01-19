using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

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
        try
        {
            var customerExists = _customerRepository.GetOne(x => x.EmailId == customer.EmailId);
            var addressExists = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);

            if (customerExists != null && addressExists != null)
            {
                Customer_AddressEntity entity = new()
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
                        // Returns true if the customer_address was created succesfully.
                        return true;
                    }
                }
                // Returns true if the customer_address already exists.
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        // Returns false if either customerExits == null or addressExists == null.
        return false;
    }

}
