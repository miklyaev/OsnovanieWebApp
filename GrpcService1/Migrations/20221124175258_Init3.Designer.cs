﻿// <auto-generated />
using System;
using GrpcService1.DbService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrpcService1.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221124175258_Init3")]
    partial class Init3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GrpcService1.DbService.Model.TGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("group_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GroupId"));

                    b.Property<string>("GroupName")
                        .HasColumnType("text")
                        .HasColumnName("group_name");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.HasKey("GroupId");

                    b.ToTable("t_group");
                });

            modelBuilder.Entity("GrpcService1.DbService.Model.TRole", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoleId"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_time");

                    b.Property<DateTime?>("OffTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("off_time");

                    b.Property<int>("RoleName")
                        .HasColumnType("integer")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId");

                    b.ToTable("t_role");
                });

            modelBuilder.Entity("GrpcService1.DbService.Model.TRoleGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("role_group_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("group_id")
                        .HasColumnType("integer");

                    b.Property<int?>("role_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("group_id");

                    b.HasIndex("role_id");

                    b.ToTable("t_role_group");
                });

            modelBuilder.Entity("GrpcService1.DbService.Model.TUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<int?>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_time");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<DateTime?>("OffTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("off_time");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text")
                        .HasColumnName("patronymic");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("UserId");

                    b.ToTable("t_user");
                });

            modelBuilder.Entity("GrpcService1.DbService.Model.TUserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_role_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("role_id")
                        .HasColumnType("integer");

                    b.Property<int>("user_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("role_id");

                    b.HasIndex("user_id");

                    b.ToTable("t_user_role");
                });

            modelBuilder.Entity("GrpcService1.DbService.Model.TRoleGroup", b =>
                {
                    b.HasOne("GrpcService1.DbService.Model.TGroup", "Group")
                        .WithMany()
                        .HasForeignKey("group_id");

                    b.HasOne("GrpcService1.DbService.Model.TRole", "Role")
                        .WithMany()
                        .HasForeignKey("role_id");

                    b.Navigation("Group");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GrpcService1.DbService.Model.TUserRole", b =>
                {
                    b.HasOne("GrpcService1.DbService.Model.TRole", "Role")
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrpcService1.DbService.Model.TUser", "User")
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
