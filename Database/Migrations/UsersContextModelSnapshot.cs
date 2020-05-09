﻿// <auto-generated />
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(UsersContext))]
    partial class UsersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("Database.User", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.HasKey("Login");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
