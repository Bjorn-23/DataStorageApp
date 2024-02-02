using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class Customer_AddressRepository(DataContext context) : BaseRepository<Customer_AddressEntity, DataContext>(context), ICustomer_AddressRepository
{
    private readonly DataContext _context = context;

    public override IEnumerable<Customer_AddressEntity> GetAll()
    {
        try
        {
            var existingCustomer_AddressDetails = _context.Customer_Addresses
                .Include(i => i.Address)
                .Include(i => i.Customer)
                .ToList();

            if (existingCustomer_AddressDetails != null)
            {
                return existingCustomer_AddressDetails;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public override Customer_AddressEntity GetOne(Expression<Func<Customer_AddressEntity, bool>> predicate)
    {
        try
        {
            var existingCustomer_AddressDetails = _context.Customer_Addresses
                .Where(predicate)
                .Include(i => i.Address)
                .Include(i => i.Customer)
                .FirstOrDefault();
            if (existingCustomer_AddressDetails != null)
            {
                return existingCustomer_AddressDetails;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
