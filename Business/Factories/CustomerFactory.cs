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
                Id = dto.Id,
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
                Id = entity.Id,
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

    public static CustomerDetailsDto CreateCustomerDetails(CustomerEntity cEntity, UserEntity uEntity)
    {
        try
        {
            return new CustomerDetailsDto
            {
                Id  = cEntity.Id,
                FirstName = cEntity.FirstName,
                LastName = cEntity.LastName,
                EmailId = cEntity.EmailId,
                PhoneNumber = cEntity.PhoneNumber,
                UserRoleName = uEntity.UserRoleName
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
