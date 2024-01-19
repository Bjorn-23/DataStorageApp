//using Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Contexts;

//public partial class ProductCatalogContext(DbContextOptions options) : DbContext(options)
//{


//    // ALL THIS WORKS; MOVE logic To Database First file
//    public virtual DbSet<PriceListEntity> PriceLists { get; set; }

//    public virtual DbSet<CategoryEntity> Categories { get; set; }

//    public virtual DbSet<ProductEntity> Products { get; set; }



//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        // ALL THIS WORKS; MOVE logic To Database First file
//        //---------------------------------------Products
//        modelBuilder.Entity<ProductEntity>()
//                .HasKey(x => x.ArticleNumber);


//        modelBuilder.Entity<ProductEntity>()
//                .HasOne(x => x.Category)
//                .WithMany()
//                .HasForeignKey(x => x.CategoryId)
//                .OnDelete(DeleteBehavior.Cascade);


//        modelBuilder.Entity<PriceListEntity>()
//                .HasKey(x => x.ArticleNumber);


//        modelBuilder.Entity<PriceListEntity>()
//                .HasOne(x => x.Product)
//                .WithMany()
//                .HasForeignKey(x => x.ArticleNumber)
//                .OnDelete(DeleteBehavior.Cascade);


//        modelBuilder.Entity<CategoryEntity>()
//                .HasKey(x => x.Id);


//        modelBuilder.Entity<CategoryEntity>()
//                .HasMany(category => category.Product)
//                .WithOne(product => product.Category)
//                .HasForeignKey(product => product.CategoryId)
//                .OnDelete(DeleteBehavior.Cascade);
//        //---------------------------------------Products
//    }


//}
