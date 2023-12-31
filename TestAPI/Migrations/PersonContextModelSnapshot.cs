﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestAPI.DAL;

#nullable disable

namespace TestAPI.Migrations
{
    [DbContext(typeof(PersonContext))]
    partial class PersonContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestAPI.Model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int>("Id"), 100L, null, null, null, null, null);

                    b.Property<int>("Counter")
                        .HasColumnType("integer");

                    b.Property<string>("FavouriteColour")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<int>("NumberChildren")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Counter = 0,
                            FavouriteColour = "blue",
                            FirstName = "Mike",
                            LastName = "Dragert",
                            NumberChildren = 2,
                            Username = "MikieD"
                        },
                        new
                        {
                            Id = 2,
                            Counter = 0,
                            FavouriteColour = "blue",
                            FirstName = "James",
                            LastName = "Young",
                            NumberChildren = 2,
                            Username = "James"
                        },
                        new
                        {
                            Id = 3,
                            Counter = 0,
                            FavouriteColour = "green",
                            FirstName = "Ross",
                            LastName = "Struthers",
                            NumberChildren = 0,
                            Username = "Rossco"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
