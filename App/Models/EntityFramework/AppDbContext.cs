using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace App.Models.EntityFramework;

public partial class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=R508;uid=postgres;password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.IdProduct);
            
            e.HasOne(p => p.BrandNavigation)
                .WithMany(m => m.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_products_brand");
            
            e.HasOne(p => p.ProductTypeNavigation)
                .WithMany(m => m.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_products_product_type");
        });
        modelBuilder.Entity<ProductType>(e =>
        {
            e.HasKey(p => p.IdProductType);

            e.HasMany(p => p.Products)
                .WithOne(m => m.ProductTypeNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        modelBuilder.Entity<Brand>(e =>
        {
            e.HasKey(p => p.IdBrand);

            e.HasMany(p => p.Products)
                .WithOne(m => m.BrandNavigation)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
