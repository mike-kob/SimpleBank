using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atm",
                columns: table => new
                {
                    AtmId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RemainingMoney = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atm", x => x.AtmId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DateBirth = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    CardNum = table.Column<string>(nullable: false),
                    Pin = table.Column<string>(maxLength: 4, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Balance = table.Column<decimal>(nullable: true),
                    OwnMoney = table.Column<decimal>(nullable: true),
                    Limit = table.Column<decimal>(nullable: true),
                    IsInLimit = table.Column<bool>(nullable: true),
                    LimitWithdrawn = table.Column<DateTime>(nullable: true),
                    MinSum = table.Column<decimal>(nullable: true),
                    DepositCard_Balance = table.Column<decimal>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    TotalBalance = table.Column<decimal>(nullable: true),
                    StartDeposit = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardNum);
                    table.ForeignKey(
                        name: "FK_Card_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TxnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfTxn = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    DatetimeOfTxn = table.Column<DateTime>(nullable: false),
                    Success = table.Column<bool>(nullable: false),
                    CardSenderNum = table.Column<string>(nullable: true),
                    CardReceiverNum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TxnId);
                    table.ForeignKey(
                        name: "FK_Transaction_Card_CardReceiverNum",
                        column: x => x.CardReceiverNum,
                        principalTable: "Card",
                        principalColumn: "CardNum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Card_CardSenderNum",
                        column: x => x.CardSenderNum,
                        principalTable: "Card",
                        principalColumn: "CardNum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Card_UserId",
                table: "Card",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CardReceiverNum",
                table: "Transaction",
                column: "CardReceiverNum");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CardSenderNum",
                table: "Transaction",
                column: "CardSenderNum");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atm");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
