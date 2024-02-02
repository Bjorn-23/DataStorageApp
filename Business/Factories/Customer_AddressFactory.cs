﻿using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public class Customer_AddressFactory
{
    public static Customer_AddressEntity Create(Customer_AddressDto dto)
    {
        try
        {
            return new Customer_AddressEntity
            {
                AddressId = dto.AddressId,
                Address = dto.Address,
                CustomerId = dto.CustomerId,
                Customer = dto.Customer
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static Customer_AddressDto Create(Customer_AddressEntity entity)
    {
        try
        {
            return new Customer_AddressDto
            {
                AddressId = entity.AddressId,
                Address = entity.Address,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static IEnumerable<Customer_AddressDto> Create(IEnumerable<Customer_AddressEntity> dtos)
    {
        try
        {
            List<Customer_AddressDto> list = new();

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

