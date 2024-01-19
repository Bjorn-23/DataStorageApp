using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class CustomerFactory
{
    public static CustomerEntity Create(CustomerDto dto)
    {
        try
        {
            return new CustomerEntity
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailId = dto.EmailId,
                PhoneNumber = dto.PhoneNumber,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static CustomerDto Create(CustomerEntity entity)
    {
        try
        {
            return new CustomerDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                EmailId = entity.EmailId,
                PhoneNumber = entity.PhoneNumber,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<CustomerEntity> Create(IEnumerable<CustomerDto> dtos)
    {
        try
        {
            List<CustomerEntity> list = new();

            foreach (var dto in dtos)
            {
                list.Add(Create(dto));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<CustomerDto> Create(IEnumerable<CustomerEntity> entities)
    {
        try
        {
            List<CustomerDto> list = new();

            foreach (var entity in entities)
            {
                list.Add(Create(entity));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static CustomerDetailsDto CreateCustomerDetails(CustomerEntity entity, UserDto uDto)
    {
        try
        {
            return new CustomerDetailsDto
            {
                Id  = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                EmailId = entity.EmailId,
                PhoneNumber = entity.PhoneNumber,
                UserRoleName = uDto.UserRoleName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
