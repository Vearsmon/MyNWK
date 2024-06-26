﻿// <auto-generated />
using System;
using Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    partial class CoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Objects.Categories.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Markets.Market", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Closed")
                        .HasColumnType("boolean")
                        .HasColumnName("closed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasColumnName("name");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer")
                        .HasColumnName("owner_id");

                    b.HasKey("Id")
                        .HasName("pk_markets");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_markets_owner_id");

                    b.ToTable("markets", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Markets.MarketInfo", b =>
                {
                    b.Property<int>("MarketId")
                        .HasColumnType("integer")
                        .HasColumnName("market_id");

                    b.Property<bool>("AutoHide")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("auto_hide");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<TimeOnly?>("WorksFrom")
                        .HasColumnType("time without time zone")
                        .HasColumnName("works_from");

                    b.Property<TimeOnly?>("WorksTo")
                        .HasColumnType("time without time zone")
                        .HasColumnName("works_to");

                    b.HasKey("MarketId")
                        .HasName("pk_market_infos");

                    b.ToTable("market_infos", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Products.Product", b =>
                {
                    b.Property<int>("MarketId")
                        .HasColumnType("integer")
                        .HasColumnName("market_id");

                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamptz")
                        .HasColumnName("created_at");

                    b.Property<string>("ImageLocation")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("image_location");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<int>("Remained")
                        .HasColumnType("integer")
                        .HasColumnName("remained");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasColumnName("title");

                    b.HasKey("MarketId", "ProductId")
                        .HasName("pk_products");

                    b.HasIndex("CategoryId")
                        .IsUnique()
                        .HasDatabaseName("ix_products_category_id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Rooms.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FrameNumber")
                        .HasColumnType("integer")
                        .HasColumnName("frame_number");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer")
                        .HasColumnName("room_number");

                    b.Property<char>("Section")
                        .HasColumnType("character(1)")
                        .HasColumnName("section");

                    b.HasKey("Id")
                        .HasName("pk_rooms");

                    b.ToTable("rooms", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Sellers.Seller", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer")
                        .HasColumnName("room_id");

                    b.Property<bool>("ShowRoom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("show_room");

                    b.HasKey("UserId")
                        .HasName("pk_sellers");

                    b.HasIndex("RoomId")
                        .IsUnique()
                        .HasDatabaseName("ix_sellers_room_id");

                    b.ToTable("sellers", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("varchar(64)")
                        .HasColumnName("name");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(16)")
                        .HasColumnName("phone_number");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegram_id");

                    b.Property<string>("TelegramUsername")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("telegram_username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("TelegramId")
                        .HasDatabaseName("ix_users_telegram_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Core.Objects.Markets.Market", b =>
                {
                    b.HasOne("Core.Objects.Sellers.Seller", "Seller")
                        .WithMany("Markets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_markets_sellers_owner_id");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("Core.Objects.Markets.MarketInfo", b =>
                {
                    b.HasOne("Core.Objects.Markets.Market", null)
                        .WithOne("MarketInfo")
                        .HasForeignKey("Core.Objects.Markets.MarketInfo", "MarketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_market_infos_markets_market_id");
                });

            modelBuilder.Entity("Core.Objects.Products.Product", b =>
                {
                    b.HasOne("Core.Objects.Categories.Category", "Category")
                        .WithOne()
                        .HasForeignKey("Core.Objects.Products.Product", "CategoryId")
                        .HasConstraintName("fk_products_categories_category_id");

                    b.HasOne("Core.Objects.Markets.Market", "Market")
                        .WithMany("Products")
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_markets_market_id");

                    b.Navigation("Category");

                    b.Navigation("Market");
                });

            modelBuilder.Entity("Core.Objects.Sellers.Seller", b =>
                {
                    b.HasOne("Core.Objects.Rooms.Room", "Room")
                        .WithOne()
                        .HasForeignKey("Core.Objects.Sellers.Seller", "RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sellers_rooms_room_id");

                    b.HasOne("Core.Objects.Users.User", "User")
                        .WithOne("Seller")
                        .HasForeignKey("Core.Objects.Sellers.Seller", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_sellers_users_user_id");

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Objects.Markets.Market", b =>
                {
                    b.Navigation("MarketInfo")
                        .IsRequired();

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Core.Objects.Sellers.Seller", b =>
                {
                    b.Navigation("Markets");
                });

            modelBuilder.Entity("Core.Objects.Users.User", b =>
                {
                    b.Navigation("Seller");
                });
#pragma warning restore 612, 618
        }
    }
}
