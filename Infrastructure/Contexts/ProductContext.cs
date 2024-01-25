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
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC070F34871F");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07775DCDF6");
        });

        modelBuilder.Entity<OrderRowEntity>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrderId }).HasName("PK__OrderRow__DE2DE9BBED0F1B0C");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ArticleNumberNavigation).WithMany(p => p.OrderRows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Artic__69FBBC1F");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Order__6AEFE058");
        });

        modelBuilder.Entity<PriceListEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PriceLis__3214EC0747FED646");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__Products__3C9911439815FBF1");

            entity.HasOne(d => d.CategoryNameNavigation).WithMany(p => p.Products)
                .HasPrincipalKey(p => p.CategoryName)
                .HasForeignKey(d => d.CategoryName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__690797E6");

            entity.HasOne(d => d.Price).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__PriceI__681373AD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
