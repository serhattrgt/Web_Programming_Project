using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Models;

namespace Web_Programming_Proje.Data;

public partial class StoreDbContext : DbContext
{
    public StoreDbContext()
    {
    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; } =null!;

    public virtual DbSet<Category> Categories { get; set; }=null!;

    public virtual DbSet<Order> Orders { get; set; }=null!;

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }=null!;

    public virtual DbSet<Payment> Payments { get; set; }=null!;

    public virtual DbSet<Product> Products { get; set; }=null!;

    public virtual DbSet<Role> Roles { get; set; }=null!;

    public virtual DbSet<User> Users { get; set; }=null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressID).HasName("Addresses_pkey");

            entity.Property(e => e.AddressID).HasColumnName("AddressID");

             entity.HasMany(p => p.Users) 
            .WithOne(d => d.Address)
            .HasForeignKey(d => d.AddressID)
            .OnDelete(DeleteBehavior.Cascade) 
            .HasConstraintName("FK_Address_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("Categories_pkey");

            entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("Orders_pkey");

            entity.Property(e => e.OrderID).HasColumnName("OrderID");
            entity.Property(e => e.DeliveryDate)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP + '2 days'::interval)")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsDelivered).HasDefaultValue(false);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.PaymentID).HasColumnName("PaymentID");
            entity.Property(e => e.UserID).HasColumnName("UserID");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Order");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK_User_Order");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.OrderProductID).HasName("OrderProduct_pkey");

            entity.ToTable("OrderProduct");

            entity.Property(e => e.OrderProductID).HasColumnName("OrderProductID");
            entity.Property(e => e.OrderID).HasColumnName("OrderID");
            entity.Property(e => e.ProductID).HasColumnName("ProductID");
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderID)
                .HasConstraintName("FK_Order_Product");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Product_Order");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("Payments_pkey");

            entity.Property(e => e.PaymentID).HasColumnName("PaymentID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("Products_pkey");

            entity.Property(e => e.ProductID).HasColumnName("ProductID");
            entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
            entity.Property(e => e.Price).HasPrecision(10, 2);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Category");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("Roles_pkey");

            entity.Property(e => e.RoleID).HasColumnName("RoleID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("Users_pkey");

            entity.HasIndex(e => e.UserName, "Users_UserName_key").IsUnique();

            entity.Property(e => e.UserID).HasColumnName("UserID");
            entity.Property(e => e.AddressID).HasColumnName("AddressID");

            entity.HasOne(d => d.Address)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AddressID)
                    .OnDelete(DeleteBehavior.Cascade) // User silindiğinde Address de silinsin
                    .HasConstraintName("FK_User_Address");


            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleID")
                        .HasConstraintName("FK_Role_User"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserID")
                        .HasConstraintName("FK_User_Role"),
                    j =>
                    {
                        j.HasKey("UserID", "RoleID").HasName("UserRole_pkey");
                        j.ToTable("UserRole");
                        j.IndexerProperty<long>("UserID").HasColumnName("UserID");
                        j.IndexerProperty<long>("RoleID").HasColumnName("RoleID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
