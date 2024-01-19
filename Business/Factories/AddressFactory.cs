using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class AddressFactory
{
    public static AddressEntity Create(AddressDto dto)
    {
        try
        {
            return new AddressEntity
            {
                Id = dto.Id,
                StreetName = dto.StreetName,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static AddressDto Create(AddressEntity entity)
    {
        try
        {
            return new AddressDto
            {
                Id = entity.Id,
                StreetName = entity.StreetName,
                City = entity.City,
                PostalCode = entity.PostalCode,
                Country = entity.Country,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<AddressEntity> Create(IEnumerable<AddressDto> dtos)
    {
        try
        {
            List<AddressEntity> list = new();

            foreach (var dto in dtos)
            {
                list.Add(Create(dto));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<AddressDto> Create(IEnumerable<AddressEntity> dtos)
    {
        try
        {
            List<AddressDto> list = new();

            foreach (var dto in dtos)
            {
                list.Add(Create(dto));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
