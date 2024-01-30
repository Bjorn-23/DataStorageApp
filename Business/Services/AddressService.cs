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
    private readonly UserService _userService;

    public AddressService(IAddressRepository addressRepository, ICustomer_AddressRepository customer_AddressRepository, UserService userService)
    {
        _addressRepository = addressRepository;
        _customer_AddressRepository = customer_AddressRepository;
        _userService = userService;
    }

    public AddressDto CreateAddress(AddressDto address)
    {
        try
        {
            var existingAddress = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            if (existingAddress != null)
            {
                var addressDto = AddressFactory.Create(existingAddress);
                return addressDto;
            }

            AddressEntity addressEntity = new()
            {
                StreetName = address.StreetName,
                PostalCode = address.PostalCode,
                City = address.City,
                Country = address.Country,
            };

            var result = _addressRepository.Create(addressEntity);
            if (result != null)
                 return address;

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

                    AddressDto addressDto = new()
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

    public AddressDto UpdateAddress(AddressDto address, AddressDto newAddressDetails)
    {
        try
        {
            var existingAddress = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            var checkRole = _userService.FindRoleOfActiveUser();

            if (existingAddress != null && checkRole.UserRoleName == "Admin")
            {
                AddressEntity addressEntity = new()
                {
                    Id = existingAddress.Id,
                    StreetName = string.IsNullOrWhiteSpace(newAddressDetails.StreetName) ? existingAddress.StreetName : newAddressDetails.StreetName,
                    PostalCode = string.IsNullOrWhiteSpace(newAddressDetails.PostalCode) ? existingAddress.PostalCode : newAddressDetails.PostalCode,
                    City = string.IsNullOrWhiteSpace(newAddressDetails.City) ? existingAddress.City : newAddressDetails.City,
                    Country = string.IsNullOrWhiteSpace(newAddressDetails.Country) ? existingAddress.Country : newAddressDetails.Country,
                };

                var result = _addressRepository.Update(x => x.Id == existingAddress.Id, addressEntity);
                if (result != null)
                {
                    AddressDto addressDto = AddressFactory.Create(result);
                    return addressDto;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public AddressDto DeleteAddress(AddressDto address)
    {
        try
        {
            var existingAddress = _addressRepository.GetOne(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            var checkRole = _userService.FindRoleOfActiveUser();

            if (existingAddress != null && checkRole.UserRoleName == "Admin")
            {
                var result = _addressRepository.Delete(x => x.Id == existingAddress.Id);

                if (result)
                {
                    var existingDto = AddressFactory.Create(existingAddress);
                    return existingDto;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}