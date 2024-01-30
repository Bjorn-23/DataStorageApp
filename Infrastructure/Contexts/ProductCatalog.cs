using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Contexts;

public partial class ProductCatalog(DbContextOptions<ProductCatalog> options) : DbContext(options)
{
    public virtual DbSet<CategoryEntity> Categories { get; set; }

    public virtual DbSet<OrderEntity> Orders { get; set; }

    public virtual DbSet<OrderRowEntity> OrderRows { get; set; }

    public virtual DbSet<PriceListEntity> PriceLists { get; set; }

    public virtual DbSet<ProductEntity> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC070CC72E51");
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC0767723072");
        });

        modelBuilder.Entity<OrderRowEntity>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrderId }).HasName("PK__OrderRow__DE2DE9BB07251A08");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.ArticleNumberNavigation).WithOne(p => p.OrderRow).HasConstraintName("FK__OrderRows__Artic__2704CA5F");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRows).HasConstraintName("FK__OrderRows__Order__27F8EE98");
        });

        modelBuilder.Entity<PriceListEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PriceLis__3214EC07E95AB0C2");
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ArticleNumber).HasName("PK__Products__3C9911434C0730FC");

            entity.HasOne(d => d.CategoryNameNavigation).WithMany(p => p.Products)
                .HasPrincipalKey(p => p.CategoryName)
                .HasForeignKey(d => d.CategoryName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__2610A626");

            entity.HasOne(d => d.Price).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__PriceI__251C81ED");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}