using Business.Dtos;
using Business.Factories;
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

    public bool CreateCustomer_Address(CustomerDto customer, AddressDto address)
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

    public Customer_AddressDto GetCustomer_Address(CustomerDto customer, AddressDto address)
    {
        try
        {
            var existingCustomer_Address = _customer_AddressRepository.GetOne(x => x.Customer.EmailId == customer.EmailId && x.Address.StreetName == address.StreetName && x.Address.PostalCode == address.PostalCode); 

            if (existingCustomer_Address != null)
            {
                return Customer_AddressFactory.Create(existingCustomer_Address);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<Customer_AddressDto> GetAlLCustomer_Addresses()
    {
        try
        {
            var existingCustomer_Addresses = _customer_AddressRepository.GetAll();
            if (existingCustomer_Addresses.Count() > 0)
            {
                return Customer_AddressFactory.Create(existingCustomer_Addresses);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public Customer_AddressDto UpdateCustomer_Address(CustomerDto existingCustomer, CustomerDto updatedCustomer, AddressDto existingAddress, AddressDto updatedAddress)
    {
        try
        {
            var customerExists = _customerRepository.GetOne(x => x.EmailId == existingCustomer.EmailId);
            var addressExists = _addressRepository.GetOne(x => x.StreetName == existingAddress.StreetName && x.PostalCode == existingAddress.PostalCode);

            if (customerExists != null && addressExists != null)
            {

                var customer_AddressExists = _customer_AddressRepository.GetOne(x => x.AddressId == addressExists.Id && x.CustomerId == customerExists.Id);

                if (customer_AddressExists != null)
                {
                    Customer_AddressEntity updatedCustomer_Address = new()
                    {
                        CustomerId = !string.IsNullOrWhiteSpace(updatedCustomer.Id) ? updatedCustomer.Id : customer_AddressExists.CustomerId,
                        AddressId = !string.IsNullOrWhiteSpace(updatedAddress.Id.ToString()) ? updatedAddress.Id : customer_AddressExists.AddressId,
                    };

                    var result = _customer_AddressRepository.Update(customer_AddressExists, updatedCustomer_Address);
                    if (result != null)
                    {
                        return new Customer_AddressDto()
                        {
                            CustomerId = result.CustomerId,
                            AddressId = result.AddressId,
                        };
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public Customer_AddressDto DeleteCustomer_Address(CustomerDto customer, AddressDto address)
    {
        try
        {
            var existingCustomer_Address = _customer_AddressRepository.GetOne(x => x.Customer.EmailId == customer.EmailId && x.Address.StreetName == address.StreetName && x.Address.PostalCode == address.PostalCode);
            if (existingCustomer_Address != null)
            {
                var result = _customer_AddressRepository.Delete(existingCustomer_Address);
                if (result)
                    return Customer_AddressFactory.Create(existingCustomer_Address);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public Customer_AddressDto DeleteCustomer_Address(Customer_AddressDto existingCustomer_Address)
    {
        try
        {
            if (existingCustomer_Address != null)
            {
                var result = _customer_AddressRepository.Delete(Customer_AddressFactory.Create(existingCustomer_Address));
                if (result)
                    return existingCustomer_Address;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
