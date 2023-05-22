﻿// <auto-generated />
using System;
using Disasters.Api.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Disasters.Api.Migrations
{
    [DbContext(typeof(DisastersDbContext))]
    partial class DisastersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Disasters.Api.Db.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("KeyValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("Disasters.Api.Db.Disaster", b =>
                {
                    b.Property<Guid>("DisasterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Occured")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DisasterId");

                    b.ToTable("Disasters");

                    b.HasData(
                        new
                        {
                            DisasterId = new Guid("694be870-2024-41a5-b08a-5054b431b4c2"),
                            Occured = new DateTimeOffset(new DateTime(2023, 5, 22, 11, 29, 15, 541, DateTimeKind.Unspecified).AddTicks(6710), new TimeSpan(0, 2, 0, 0, 0)),
                            Summary = "Seed"
                        });
                });

            modelBuilder.Entity("Disasters.Api.Db.DisasterLocation", b =>
                {
                    b.Property<Guid>("DisasterLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DisasterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DisasterLocationId");

                    b.HasIndex("DisasterId");

                    b.HasIndex("LocationId");

                    b.ToTable("DisasterLocations");

                    b.HasData(
                        new
                        {
                            DisasterLocationId = new Guid("8feb0a12-5317-490c-a4a4-3d8d5c1328c8"),
                            DisasterId = new Guid("694be870-2024-41a5-b08a-5054b431b4c2"),
                            LocationId = new Guid("49c6030e-9dd8-4155-8f65-47394943b804")
                        });
                });

            modelBuilder.Entity("Disasters.Api.Db.Location", b =>
                {
                    b.Property<Guid>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            LocationId = new Guid("49c6030e-9dd8-4155-8f65-47394943b804"),
                            Country = "Sweden"
                        });
                });

            modelBuilder.Entity("Disasters.Api.Db.TestView", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToView("TestView");
                });

            modelBuilder.Entity("Disasters.Api.Db.DisasterLocation", b =>
                {
                    b.HasOne("Disasters.Api.Db.Disaster", "Disaster")
                        .WithMany("DisasterLocations")
                        .HasForeignKey("DisasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Disasters.Api.Db.Location", "Location")
                        .WithMany("DisasterLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disaster");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Disasters.Api.Db.Disaster", b =>
                {
                    b.Navigation("DisasterLocations");
                });

            modelBuilder.Entity("Disasters.Api.Db.Location", b =>
                {
                    b.Navigation("DisasterLocations");
                });
#pragma warning restore 612, 618
        }
    }
}
