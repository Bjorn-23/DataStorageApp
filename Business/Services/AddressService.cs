using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Business.Services;

public class AddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly ICustomer_AddressRepository _customer_AddressRepository;

    public AddressService(IAddressRepository addressRepository, ICustomer_AddressRepository customer_AddressRepository)
    {
        _addressRepository = addressRepository;
        _customer_AddressRepository = customer_AddressRepository;
    }

    public bool AddressExists (AddressDto address)
    {
        var exists = _addressRepository.Exists(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
        return exists;
    }

    public AddressDto CreateAddress(AddressDto address)
    {
        var exists = _addressRepository.Exists(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
        if (exists)
        {
            return address;
        }

        AddressEntity entity = new()
        {
            StreetName = address.StreetName,
            PostalCode = address.PostalCode,
            City = address.City,
            Country = address.Country,
        };

        var result = _addressRepository.Create(entity);
        if (result != null)
        {
            return address;
        }
        
        return null!;
    }

    public IEnumerable<AddressDto> GetOneAddressWithCustomerId(CustomerEntity customer)
    {

        var allCustomerAddresses = _customer_AddressRepository.GetAllWithPredicate(x => x.CustomerId == customer.Id);

        if (allCustomerAddresses != null)
        {
            List<AddressDto> addressDtos = new();

            foreach (var addressId in allCustomerAddresses)
            {
                var address = _addressRepository.GetOne(x => x.Id == addressId.AddressId);

                var addressDto = new AddressDto ()
                {
                    StreetName = address.StreetName,
                    PostalCode = address.PostalCode,
                    City = address.City,
                    Country = address.Country,
                };

                addressDtos.Add(addressDto);
            }

            return addressDtos;
        }
        return null!;
    }

}
