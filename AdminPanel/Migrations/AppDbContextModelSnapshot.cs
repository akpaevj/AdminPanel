﻿// <auto-generated />
using System;
using AdminPanel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdminPanel.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdminPanel.Models.InfoBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConnectionType")
                        .HasColumnType("int");

                    b.Property<string>("IBasesContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InfoBaseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Server")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InfoBases");
                });

            modelBuilder.Entity("AdminPanel.Models.InfoBaseInfoBasesList", b =>
                {
                    b.Property<Guid>("InfoBaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("InfoBasesListId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("InfoBaseId", "InfoBasesListId");

                    b.HasIndex("InfoBasesListId");

                    b.ToTable("InfoBaseInfoBasesLists");
                });

            modelBuilder.Entity("AdminPanel.Models.InfoBasesList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InfoBasesLists");
                });

            modelBuilder.Entity("AdminPanel.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("InfoBasesListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SamAccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sid")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InfoBasesListId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AdminPanel.Models.InfoBaseInfoBasesList", b =>
                {
                    b.HasOne("AdminPanel.Models.InfoBase", "InfoBase")
                        .WithMany("InfoBaseInfoBasesLists")
                        .HasForeignKey("InfoBaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdminPanel.Models.InfoBasesList", "InfoBasesList")
                        .WithMany("InfoBaseInfoBasesLists")
                        .HasForeignKey("InfoBasesListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AdminPanel.Models.User", b =>
                {
                    b.HasOne("AdminPanel.Models.InfoBasesList", "InfoBasesList")
                        .WithMany("Users")
                        .HasForeignKey("InfoBasesListId");
                });
#pragma warning restore 612, 618
        }
    }
}