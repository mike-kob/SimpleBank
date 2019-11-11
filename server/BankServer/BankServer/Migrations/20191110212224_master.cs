using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankServer.Migrations
{
    public partial class master : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commission",
                table: "DepositCard");

            migrationBuilder.DropColumn(
                name: "IsLimitPaid",
                table: "CreditCard");

            migrationBuilder.AddColumn<int>(
                name: "TxnId",
                table: "DepositCard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TxnId",
                table: "CheckingCard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TxnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfTxn = table.Column<int>(nullable: false),
                    CardSender = table.Column<long>(nullable: false),
                    CardReceiver = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    DatetimeOfTxn = table.Column<DateTime>(nullable: false),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TxnId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositCard_TxnId",
                table: "DepositCard",
                column: "TxnId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckingCard_TxnId",
                table: "CheckingCard",
                column: "TxnId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckingCard_Transaction_TxnId",
                table: "CheckingCard",
                column: "TxnId",
                principalTable: "Transaction",
                principalColumn: "TxnId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepositCard_Transaction_TxnId",
                table: "DepositCard",
                column: "TxnId",
                principalTable: "Transaction",
                principalColumn: "TxnId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckingCard_Transaction_TxnId",
                table: "CheckingCard");

            migrationBuilder.DropForeignKey(
                name: "FK_DepositCard_Transaction_TxnId",
                table: "DepositCard");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_DepositCard_TxnId",
                table: "DepositCard");

            migrationBuilder.DropIndex(
                name: "IX_CheckingCard_TxnId",
                table: "CheckingCard");

            migrationBuilder.DropColumn(
                name: "TxnId",
                table: "DepositCard");

            migrationBuilder.DropColumn(
                name: "TxnId",
                table: "CheckingCard");

            migrationBuilder.AddColumn<bool>(
                name: "Commission",
                table: "DepositCard",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLimitPaid",
                table: "CreditCard",
                type: "bit",
                nullable: true);
        }
    }
}
