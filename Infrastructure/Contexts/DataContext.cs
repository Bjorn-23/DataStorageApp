using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

    public virtual DbSet<AddressEntity> Address { get; set; }
    
    public virtual DbSet<CustomerEntity> Customers { get; set; }

    public virtual DbSet<Customer_AddressEntity> Customer_Addresses { get; set; }

    public virtual DbSet<PriceListEntity> PriceLists { get; set; }

    public virtual DbSet<CategoryEntity> Categories { get; set; }

    public virtual DbSet<ProductEntity> Products { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //---------------------------------------Customers
        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(x =>  x.Email)
            .IsUnique();

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
        //---------------------------------------Customers

        //---------------------------------------Products
        modelBuilder.Entity<ProductEntity>()
            .HasKey(x => x.ArticleNumber);

        modelBuilder.Entity<ProductEntity>()
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PriceListEntity>()
            .HasKey(x => x.ArticleNumber);

        modelBuilder.Entity<PriceListEntity>()
            .HasOne(priceList => priceList.Product)
            .WithOne(product => product.PriceList)
            .HasForeignKey<PriceListEntity>(p => p.ArticleNumber)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<CategoryEntity>()
            .HasKey(x => x.Id);

        //modelBuilder.Entity<CategoryEntity>()
        //    .HasOne(x => x.Product)
        //    .WithMany()
        //    .HasForeignKey(x => x.CategoryName);

        modelBuilder.Entity<CategoryEntity>()
            .HasMany(category => category.Product)
            .WithOne(product => product.Category)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); 

        //---------------------------------------Products

    }
}
