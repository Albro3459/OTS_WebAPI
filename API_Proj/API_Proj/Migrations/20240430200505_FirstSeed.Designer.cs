﻿// <auto-generated />
using System;
using API_Proj.Infastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API_Proj.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20240430200505_FirstSeed")]
    partial class FirstSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API_Proj.Domain.Entity.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"));

                    b.Property<string>("CurrentProjects")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("JobTitle")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double?>("YearsAtCompany")
                        .HasColumnType("float");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            EmployeeID = 1001,
                            CurrentProjects = "[\"Api Project\"]",
                            EmployeeName = "Alex Brodsky",
                            IsDeleted = false,
                            JobTitle = "Student Developer",
                            YearsAtCompany = 0.5
                        },
                        new
                        {
                            EmployeeID = 1002,
                            CurrentProjects = "[\"Twidling Thumbs\"]",
                            EmployeeName = "Hoa Nguyen",
                            IsDeleted = false,
                            JobTitle = "Student Developer",
                            YearsAtCompany = 0.5
                        });
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Laptop", b =>
                {
                    b.Property<int>("LaptopID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LaptopID"));

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LaptopName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LaptopID");

                    b.HasIndex("EmployeeID")
                        .IsUnique()
                        .HasFilter("[EmployeeID] IS NOT NULL");

                    b.ToTable("Laptop");

                    b.HasData(
                        new
                        {
                            LaptopID = 1001,
                            EmployeeID = 1001,
                            IsDeleted = false,
                            LaptopName = "Brodsky's Laptop"
                        },
                        new
                        {
                            LaptopID = 1002,
                            EmployeeID = 1002,
                            IsDeleted = false,
                            LaptopName = "Hoa's Laptop"
                        });
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Office", b =>
                {
                    b.Property<int>("OfficeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OfficeID"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("OfficeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("RegionID")
                        .HasColumnType("int");

                    b.HasKey("OfficeID");

                    b.HasIndex("RegionID");

                    b.ToTable("Office");

                    b.HasData(
                        new
                        {
                            OfficeID = 1001,
                            IsDeleted = false,
                            OfficeName = "Galvez Building",
                            RegionID = 1001
                        },
                        new
                        {
                            OfficeID = 1002,
                            IsDeleted = false,
                            OfficeName = "Deloitte Austin",
                            RegionID = 1002
                        });
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Region", b =>
                {
                    b.Property<int>("RegionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegionID"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("RegionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RegionID");

                    b.ToTable("Region");

                    b.HasData(
                        new
                        {
                            RegionID = 1001,
                            IsDeleted = false,
                            RegionName = "South West"
                        },
                        new
                        {
                            RegionID = 1002,
                            IsDeleted = false,
                            RegionName = "South"
                        });
                });

            modelBuilder.Entity("OfficeEmployee", b =>
                {
                    b.Property<int>("OfficesID")
                        .HasColumnType("int");

                    b.Property<int>("EmployeesID")
                        .HasColumnType("int");

                    b.HasKey("OfficesID", "EmployeesID");

                    b.HasIndex("EmployeesID");

                    b.ToTable("OfficeEmployee");

                    b.HasData(
                        new
                        {
                            OfficesID = 1001,
                            EmployeesID = 1001
                        },
                        new
                        {
                            OfficesID = 1002,
                            EmployeesID = 1001
                        },
                        new
                        {
                            OfficesID = 1001,
                            EmployeesID = 1002
                        });
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Laptop", b =>
                {
                    b.HasOne("API_Proj.Domain.Entity.Employee", "Employee")
                        .WithOne("Laptop")
                        .HasForeignKey("API_Proj.Domain.Entity.Laptop", "EmployeeID");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Office", b =>
                {
                    b.HasOne("API_Proj.Domain.Entity.Region", "Region")
                        .WithMany("Offices")
                        .HasForeignKey("RegionID");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("OfficeEmployee", b =>
                {
                    b.HasOne("API_Proj.Domain.Entity.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API_Proj.Domain.Entity.Office", null)
                        .WithMany()
                        .HasForeignKey("OfficesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Employee", b =>
                {
                    b.Navigation("Laptop");
                });

            modelBuilder.Entity("API_Proj.Domain.Entity.Region", b =>
                {
                    b.Navigation("Offices");
                });
#pragma warning restore 612, 618
        }
    }
}