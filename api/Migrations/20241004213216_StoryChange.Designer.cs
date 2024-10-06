﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using repository.Context;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(TurningPointsContext))]
    [Migration("20241004213216_StoryChange")]
    partial class StoryChange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("entities.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("End")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("NextChapters")
                        .IsRequired()
                        .HasColumnType("varchar(30)]");

                    b.Property<DateTime?>("RecordDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RecordUser")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("StoryId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.ToTable("Chapter");
                });

            modelBuilder.Entity("entities.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Process")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("RecordDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RecordUser")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("entities.Login", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("Blocked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("InvalidLoginCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<sbyte>("Permission")
                        .HasColumnType("tinyint(2)");

                    b.Property<DateTime?>("RecordDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RecordUser")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("TokenExpiration")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("entities.ParameterInternal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("RecordDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RecordUser")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("ParameterInternal");
                });

            modelBuilder.Entity("entities.Story", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RecommendedAge")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RecordDate")
                        .HasColumnType("datetime");

                    b.Property<string>("RecordUser")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Story");
                });

            modelBuilder.Entity("entities.Chapter", b =>
                {
                    b.HasOne("entities.Story", "Story")
                        .WithMany("Chapter")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");
                });

            modelBuilder.Entity("entities.Story", b =>
                {
                    b.Navigation("Chapter");
                });
#pragma warning restore 612, 618
        }
    }
}
