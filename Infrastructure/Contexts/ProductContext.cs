using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class ProductContext : DbContext
{
    public ProductContext()
    {
    }

    public ProductContext(DbContextOptions<ProductContext> options)
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
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC072C6E1A21");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC0779AE37D8");
        });

        modelBuilder.Entity<OrderRowEntity>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrderId }).HasName("PK__OrderRow__DE2DE9BB89CA3169");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.OrderRow)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Artic__13F1F5EB");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Order__14E61A24");
        });

        modelBuilder.Entity<PriceListEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PriceLis__3214EC07A75DB111");
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__Products__3C991143972749B3");

            entity.HasOne(d => d.CategoryNameNavigation).WithMany(p => p.Products)
                .HasPrincipalKey(p => p.CategoryName)
                .HasForeignKey(d => d.CategoryName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__12FDD1B2");

            entity.HasOne(d => d.Price).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__PriceI__1209AD79");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
