﻿// <auto-generated />
using System;
using GyakorlatiFeladat.DataContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250616194133_UserEmail")]
    partial class UserEmail
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.6");

            modelBuilder.Entity("GyakorlatiFeladat.DataContext.Entities.ShoppingItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsNeeded")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ShoppingItems");
                });

            modelBuilder.Entity("GyakorlatiFeladat.DataContext.Entities.ShoppingItemVote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShoppingItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingItemId");

                    b.ToTable("ShoppingItemVote");
                });

            modelBuilder.Entity("GyakorlatiFeladat.DataContext.Entities.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TaskDesc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("GyakorlatiFeladat.DataContext.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskItemUser", b =>
                {
                    b.Property<int>("TasksId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TasksId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("TaskItemUser");
                });

            modelBuilder.Entity("GyakorlatiFeladat.DataContext.Entities.ShoppingItemVote", b =>
                {
                    b.HasOne("GyakorlatiFeladat.DataContext.Entities.ShoppingItem", null)
                        .WithMany("Votes")
                        .HasForeignKey("ShoppingItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskItemUser", b =>
                {
                    b.HasOne("GyakorlatiFeladat.DataContext.Entities.TaskItem", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GyakorlatiFeladat.DataContext.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GyakorlatiFeladat.DataContext.Entities.ShoppingItem", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
