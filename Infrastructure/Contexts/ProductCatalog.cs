using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductCatalog : DbContext
{
    public ProductCatalog()
    {
    }

    public ProductCatalog(DbContextOptions<ProductCatalog> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoryEntity> Categories { get; set; }

    public virtual DbSet<OrderEntity> Orders { get; set; }

    public virtual DbSet<OrderRowEntity> OrderRows { get; set; }

    public virtual DbSet<PriceListEntity> PriceLists { get; set; }

    public virtual DbSet<ProductEntity> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC079CE50912");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC077DD18141");
        });

        modelBuilder.Entity<OrderRowEntity>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrderId }).HasName("PK__OrderRow__DE2DE9BBACD13075");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.OrderRow)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Artic__53A266AC");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRows).HasConstraintName("FK__OrderRows__Order__54968AE5");
        });

        modelBuilder.Entity<PriceListEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PriceLis__3214EC07455CA151");
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__Products__3C991143CB361042");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__52AE4273");

            entity.HasOne(d => d.Price).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__PriceI__51BA1E3A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
