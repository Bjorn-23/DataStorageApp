using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System.Diagnostics;

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
        try
        {
            var exists = _addressRepository.Exists(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            return exists;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return false;
    }

    public AddressDto CreateAddress(AddressDto address)
    {
        try
        {
            var addressExists = _addressRepository.Exists(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            if (addressExists)
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
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        
        return null!;
    }

    public IEnumerable<AddressDto> GetAddressesWithCustomerId(CustomerDetailsDto customer)
    {
        try
        {
            var allCustomerAddresses = _customer_AddressRepository.GetAllWithPredicate(x => x.CustomerId == customer.Id);

            if (allCustomerAddresses.Any())
            {
                List<AddressDto> addressDtos = new();

                foreach (var addressId in allCustomerAddresses)
                {
                    var address = _addressRepository.GetOne(x => x.Id == addressId.AddressId);

                    var addressDto = new AddressDto()
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
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public AddressDto GetOneAddress(AddressDto address)
    {
        try
        {
            var existingAddress = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            if (existingAddress != null)
            {
                var addressDto = AddressFactory.Create(existingAddress);
                return addressDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<AddressDto> GetAll()
    {
        try
        {
            var addresses = _addressRepository.GetAll();
            if (addresses.Any())
            {
                var addressList = AddressFactory.Create(addresses);
                return addressList;
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

