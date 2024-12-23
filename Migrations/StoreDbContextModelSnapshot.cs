﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web_Programming_Proje.Data;

#nullable disable

namespace Web_Programming_Proje.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    partial class StoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<long>("UserID")
                        .HasColumnType("bigint")
                        .HasColumnName("UserID");

                    b.Property<long>("RoleID")
                        .HasColumnType("bigint")
                        .HasColumnName("RoleID");

                    b.HasKey("UserID", "RoleID")
                        .HasName("UserRole_pkey");

                    b.HasIndex("RoleID");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Address", b =>
                {
                    b.Property<long>("AddressID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("AddressID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("AddressID"));

                    b.Property<string>("OpenAddress")
                        .HasColumnType("text");

                    b.HasKey("AddressID")
                        .HasName("Addresses_pkey");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Category", b =>
                {
                    b.Property<long>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("CategoryID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("CategoryID"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CategoryID")
                        .HasName("Categories_pkey");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Order", b =>
                {
                    b.Property<long>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("OrderID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("OrderID"));

                    b.Property<DateTime>("DeliveryDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("(CURRENT_TIMESTAMP + '2 days'::interval)");

                    b.Property<bool>("IsDelivered")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("PaymentID")
                        .HasColumnType("bigint")
                        .HasColumnName("PaymentID");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint")
                        .HasColumnName("UserID");

                    b.HasKey("OrderID")
                        .HasName("Orders_pkey");

                    b.HasIndex("PaymentID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.OrderProduct", b =>
                {
                    b.Property<long>("OrderProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("OrderProductID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("OrderProductID"));

                    b.Property<long>("OrderID")
                        .HasColumnType("bigint")
                        .HasColumnName("OrderID");

                    b.Property<long>("ProductID")
                        .HasColumnType("bigint")
                        .HasColumnName("ProductID");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.HasKey("OrderProductID")
                        .HasName("OrderProduct_pkey");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderProduct", (string)null);
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Payment", b =>
                {
                    b.Property<long>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("PaymentID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("PaymentID"));

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PaymentID")
                        .HasName("Payments_pkey");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Product", b =>
                {
                    b.Property<long>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ProductID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ProductID"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("CategoryID")
                        .HasColumnType("bigint")
                        .HasColumnName("CategoryID");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("FuelConsume")
                        .HasColumnType("double precision");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<int>("StockAmount")
                        .HasColumnType("integer");

                    b.Property<int>("TopSpeed")
                        .HasColumnType("integer");

                    b.HasKey("ProductID")
                        .HasName("Products_pkey");

                    b.HasIndex("CategoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Role", b =>
                {
                    b.Property<long>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("RoleID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("RoleID"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoleID")
                        .HasName("Roles_pkey");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.User", b =>
                {
                    b.Property<long>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserID");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserID"));

                    b.Property<long?>("AddressID")
                        .HasColumnType("bigint")
                        .HasColumnName("AddressID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserID")
                        .HasName("Users_pkey");

                    b.HasIndex("AddressID");

                    b.HasIndex(new[] { "UserName" }, "Users_UserName_key")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("Web_Programming_Proje.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Role_User");

                    b.HasOne("Web_Programming_Proje.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_User_Role");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Order", b =>
                {
                    b.HasOne("Web_Programming_Proje.Models.Payment", "Payment")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentID")
                        .IsRequired()
                        .HasConstraintName("FK_Payment_Order");

                    b.HasOne("Web_Programming_Proje.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_User_Order");

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.OrderProduct", b =>
                {
                    b.HasOne("Web_Programming_Proje.Models.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_Product");

                    b.HasOne("Web_Programming_Proje.Models.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Product_Order");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Product", b =>
                {
                    b.HasOne("Web_Programming_Proje.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID")
                        .IsRequired()
                        .HasConstraintName("FK_Category");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.User", b =>
                {
                    b.HasOne("Web_Programming_Proje.Models.Address", "Address")
                        .WithMany("Users")
                        .HasForeignKey("AddressID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_User_Address");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Address", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Payment", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.Product", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("Web_Programming_Proje.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}