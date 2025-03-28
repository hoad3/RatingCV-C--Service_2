﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RatingCV.Data;

#nullable disable

namespace RatingCV.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250321120644_InitialCreate-11")]
    partial class InitialCreate11
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RatingCV.Model.Thong_tin_chi_tiet_ungvien.thong_tin_chi_tiet_ungvien", b =>
                {
                    b.Property<int>("ungvienid")
                        .HasColumnType("integer");

                    b.Property<string>("chung_chi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "chung_chi");

                    b.Property<List<string>>("cong_nghe")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasAnnotation("Relational:JsonPropertyName", "cong_nghe");

                    b.Property<List<string>>("data_base")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasAnnotation("Relational:JsonPropertyName", "data_base");

                    b.Property<List<string>>("framework")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasAnnotation("Relational:JsonPropertyName", "framework");

                    b.Property<string>("hoc_van")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "hoc_van");

                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("kinh_nghiem")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "kinh_nghiem");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "phone");

                    b.HasKey("ungvienid");

                    b.ToTable("thong_tin_chi_tiet_ungvien", (string)null);
                });

            modelBuilder.Entity("RatingCV.Model.cv_ungvien.cv_ungvien", b =>
                {
                    b.Property<int>("ungvienid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ungvienid"));

                    b.Property<string>("dia_chi")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "address");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "email");

                    b.Property<string>("github")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "github");

                    b.Property<string>("sdt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "phone");

                    b.Property<string>("ten_cv")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "ten_cv");

                    b.Property<string>("ten_ung_vien")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.HasKey("ungvienid");

                    b.ToTable("cv_ungvien", (string)null);
                });

            modelBuilder.Entity("RatingCV.Model.du_an.du_an", b =>
                {
                    b.Property<int>("du_an_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("du_an_id"));

                    b.Property<string>("github")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "github");

                    b.Property<string>("mo_ta")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "mo_ta");

                    b.Property<string>("ngay_bat_dau")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "ngay_bat_dau");

                    b.Property<string>("ngay_ket_thuc")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "ngay_ket_thuc");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "role");

                    b.Property<int>("team_size")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "team_size");

                    b.Property<string>("ten_du_an")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "ten_du_an");

                    b.Property<int>("userid")
                        .HasColumnType("integer");

                    b.HasKey("du_an_id");

                    b.HasIndex("userid");

                    b.ToTable("du_an", (string)null);
                });

            modelBuilder.Entity("RatingCV.Model.Thong_tin_chi_tiet_ungvien.thong_tin_chi_tiet_ungvien", b =>
                {
                    b.HasOne("RatingCV.Model.cv_ungvien.cv_ungvien", null)
                        .WithOne()
                        .HasForeignKey("RatingCV.Model.Thong_tin_chi_tiet_ungvien.thong_tin_chi_tiet_ungvien", "ungvienid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RatingCV.Model.du_an.du_an", b =>
                {
                    b.HasOne("RatingCV.Model.cv_ungvien.cv_ungvien", null)
                        .WithMany()
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
