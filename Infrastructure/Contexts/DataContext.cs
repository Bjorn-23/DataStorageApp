using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Contexts;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<UserRoleEntity> Roles { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    public virtual DbSet<CustomerEntity> Customers { get; set; }

    public virtual DbSet<AddressEntity> Address { get; set; }

    public virtual DbSet<Customer_AddressEntity> Customer_Addresses { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //----------------------Users & UserRoles--------------------

        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.UserRole)
            .WithMany(ur => ur.Users)
            .HasForeignKey(u => u.UserRoleName)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<UserEntity>()
            .HasIndex(x => x.Email)
            .IsUnique();

        //----------------------Users & UserRoles--------------------

        //---------------------Customers & Adresses------------------

        modelBuilder.Entity<CustomerEntity>()
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<CustomerEntity>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer_AddressEntity>()
            .HasKey(x => new { x.AddressId, x.CustomerId });

        modelBuilder.Entity<Customer_AddressEntity>()
            .HasOne(ca => ca.Customer)
            .WithMany(c => c.CustomerAddresses)
            .HasForeignKey(ca => ca.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer_AddressEntity>()
            .HasOne(ca => ca.Address)
            .WithMany(a => a.CustomerAddresses)
            .HasForeignKey(ca => ca.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
