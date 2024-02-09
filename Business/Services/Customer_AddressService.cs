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

    /// <summary>
    /// Creates a new customer_Address based on one CustomerId and one AddressId.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="address"></param>
    /// <returns>Bool</returns>
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

    /// <summary>
    /// Fetches customer_address from database. Needs Email, Streetname and postalcode.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="address"></param>
    /// <returns>Customer_addressDto</returns>
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

    /// <summary>
    /// Fetches all existing Customer_Addresses from database and return them.
    /// </summary>
    /// <returns>List of Customer_AddressDto</returns>
    public IEnumerable<Customer_AddressDto> GetAllCustomer_Addresses()
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

   // No update function for this as both Properties are Principal Keys and cant be updated

    /// <summary>
    /// Deletes existing Customer_Address in database.
    /// </summary>
    /// <param name="existingCustomer_Address"></param>
    /// <returns>Customer_AddresDto</returns>
    public Customer_AddressDto DeleteCustomer_Address(Customer_AddressDto existingCustomer_Address)
    {
        try
        {
            if (existingCustomer_Address != null)
            {
                var result = _customer_AddressRepository.Delete(x => x.CustomerId == existingCustomer_Address.CustomerId && x.AddressId == existingCustomer_Address.AddressId);
                if (result)
                    return existingCustomer_Address;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
