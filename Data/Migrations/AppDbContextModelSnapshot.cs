﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderConsoleApp.Data;

#nullable disable

namespace OrderConsoleApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("OrderConsoleApp.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Laptop",
                            Price = 2500m
                        },
                        new
                        {
                            Id = 2,
                            Name = "Klawiatura",
                            Price = 120m
                        },
                        new
                        {
                            Id = 3,
                            Name = "Mysz",
                            Price = 90m
                        },
                        new
                        {
                            Id = 4,
                            Name = "Monitor",
                            Price = 1000m
                        },
                        new
                        {
                            Id = 5,
                            Name = "Kaczka debuggująca",
                            Price = 66m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
