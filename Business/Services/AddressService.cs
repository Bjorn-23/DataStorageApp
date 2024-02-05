using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
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

    public (AddressDto address, IEnumerable<CustomerDto> customers) GetOneAddressWithCustomers(AddressDto address)
    {
        try
        {
            var existingAddressWithCustomers = _addressRepository.GetAllWithPredicate(x => x.StreetName == address.StreetName && x.PostalCode == address.PostalCode);
            if (existingAddressWithCustomers.Any())
            {
                List<CustomerDto> customerDtos = new();

                foreach (var existingAddress in existingAddressWithCustomers)
                {
                    AddressDto addressDto = new()
                    {
                        Id = existingAddress.Id,
                        StreetName = existingAddress.StreetName,
                        PostalCode = existingAddress.PostalCode,
                        City = existingAddress.City,
                        Country = existingAddress.Country,
                    };

                    foreach (var customer in existingAddress.CustomerAddresses)
                    {
                        CustomerDto customerDto = new()
                        {
                            Id = customer.Customer.Id,
                            FirstName = customer.Customer.FirstName,
                            LastName = customer.Customer.LastName,
                            EmailId = customer.Customer.EmailId,
                            PhoneNumber = customer.Customer.PhoneNumber,
                        };

                        customerDtos.Add(customerDto);
                    }

                    return (addressDto, customerDtos);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return (null!, null!);
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
            var checkRole = _userService.isUserActive();

            if (existingAddress != null && checkRole.UserRole.RoleName == "Admin")
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
            var checkRole = _userService.isUserActive();

            if (existingAddress != null && checkRole.UserRole.RoleName == "Admin")
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