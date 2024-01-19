using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Contexts;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<UserEntity> Users { get; set; }

    public virtual DbSet<UserRoleEntity> Roles { get; set; }
  
    public virtual DbSet<CustomerEntity> Customers { get; set; }

    public virtual DbSet<AddressEntity> Address { get; set; }
    
    public virtual DbSet<Customer_AddressEntity> Customer_Addresses { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //----------------------Users & UserRoles--------------------

        modelBuilder.Entity<UserEntity>()
            .HasOne(x => x.UserRoleName)
            .WithOne()
            .HasForeignKey<UserRoleEntity>(x => x.RoleName)
            .OnDelete(DeleteBehavior.Cascade);

        //----------------------Users & UserRoles--------------------

        //---------------------Customers & Adresses------------------

        modelBuilder.Entity<CustomerEntity>()
            .HasOne(x => x.EmailId)
            .WithOne()
            .HasForeignKey<UserEntity>(x => x.Email)
            .OnDelete(DeleteBehavior.Cascade);

        //Is this needed now that its a foreign key?
        //modelBuilder.Entity<CustomerEntity>()
        //    .HasIndex(x =>  x.Email)
        //    .IsUnique();

        modelBuilder.Entity<AddressEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<CustomerEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Customer_AddressEntity>()
            .HasKey(x => new { x.AddressId, x.CustomerId });

        modelBuilder.Entity<Customer_AddressEntity>()
            .HasOne(ca => ca.Address)
            .WithMany()
            .HasForeignKey(ca => ca.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer_AddressEntity>()
            .HasOne(ca => ca.Customer)
            .WithMany()
            .HasForeignKey(ca => ca.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        //---------------------Customers & Adresses------------------

    }
}
