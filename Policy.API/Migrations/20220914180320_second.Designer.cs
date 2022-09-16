﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Policy.API.Data;

#nullable disable

namespace Policy.API.Migrations
{
    [DbContext(typeof(PolicyDBContext))]
    [Migration("20220914180320_second")]
    partial class second
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Policy.API.Models.AvbobPolicy", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CommencementDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Installment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PolicyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PolicyType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Policies");
                });

            modelBuilder.Entity("Policy.API.Models.PolicyHolder", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IdNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Inititals")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dob")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("gender")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("PolicyHolders");
                });
#pragma warning restore 612, 618
        }
    }
}
