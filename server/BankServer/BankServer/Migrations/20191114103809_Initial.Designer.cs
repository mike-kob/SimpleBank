﻿// <auto-generated />
using System;
using BankServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankServer.Migrations
{
    [DbContext(typeof(BankServerContext))]
    [Migration("20191114103809_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BankServer.Models.Atm", b =>
                {
                    b.Property<int>("AtmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("RemainingMoney")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("AtmId");

                    b.ToTable("Atm");
                });

            modelBuilder.Entity("BankServer.Models.Card", b =>
                {
                    b.Property<string>("CardNum")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CardNum");

                    b.HasIndex("UserId");

                    b.ToTable("Card");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Card");
                });

            modelBuilder.Entity("BankServer.Models.Transaction", b =>
                {
                    b.Property<int>("TxnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CardReceiverNum")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CardSenderNum")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DatetimeOfTxn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<int>("TypeOfTxn")
                        .HasColumnType("int");

                    b.HasKey("TxnId");

                    b.HasIndex("CardReceiverNum");

                    b.HasIndex("CardSenderNum");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("BankServer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BankServer.Models.CheckingCard", b =>
                {
                    b.HasBaseType("BankServer.Models.Card");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.ToTable("CheckingCard");

                    b.HasDiscriminator().HasValue("CheckingCard");
                });

            modelBuilder.Entity("BankServer.Models.CreditCard", b =>
                {
                    b.HasBaseType("BankServer.Models.Card");

                    b.Property<bool>("IsInLimit")
                        .HasColumnType("bit");

                    b.Property<decimal>("Limit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("LimitWithdrawn")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("MinSum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OwnMoney")
                        .HasColumnType("decimal(18,2)");

                    b.ToTable("CreditCard");

                    b.HasDiscriminator().HasValue("CreditCard");
                });

            modelBuilder.Entity("BankServer.Models.DepositCard", b =>
                {
                    b.HasBaseType("BankServer.Models.Card");

                    b.Property<decimal>("Balance")
                        .HasColumnName("DepositCard_Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDeposit")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalBalance")
                        .HasColumnType("decimal(18,2)");

                    b.ToTable("DepositCard");

                    b.HasDiscriminator().HasValue("DepositCard");
                });

            modelBuilder.Entity("BankServer.Models.Card", b =>
                {
                    b.HasOne("BankServer.Models.User", "CardUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BankServer.Models.Transaction", b =>
                {
                    b.HasOne("BankServer.Models.Card", "CardReceiver")
                        .WithMany()
                        .HasForeignKey("CardReceiverNum");

                    b.HasOne("BankServer.Models.Card", "CardSender")
                        .WithMany()
                        .HasForeignKey("CardSenderNum");
                });
#pragma warning restore 612, 618
        }
    }
}
