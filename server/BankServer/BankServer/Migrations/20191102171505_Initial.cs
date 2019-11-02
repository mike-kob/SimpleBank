using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TxnId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfTxn = table.Column<int>(nullable: false),
                    CardSender = table.Column<long>(nullable: false),
                    CardReceiver = table.Column<long>(nullable: false),
                    amount = table.Column<double>(nullable: false),
                    DatetimeOfTxn = table.Column<DateTime>(nullable: false),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TxnId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DateBirth = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    NumOfCards = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckingCard",
                columns: table => new
                {
                    CardNum = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pin = table.Column<string>(maxLength: 4, nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    TxnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckingCard", x => x.CardNum);
                    table.ForeignKey(
                        name: "FK_CheckingCard_User_Id",
                        column: x => x.Id,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckingCard_Transaction_TxnId",
                        column: x => x.TxnId,
                        principalTable: "Transaction",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    CardNum = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pin = table.Column<string>(maxLength: 4, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    OwnMoney = table.Column<decimal>(nullable: false),
                    Limit = table.Column<decimal>(nullable: false),
                    IsInLimit = table.Column<bool>(nullable: false),
                    LimitWithdrawn = table.Column<DateTime>(nullable: true),
                    EndLimit = table.Column<DateTime>(nullable: true),
                    IsLimitPaid = table.Column<bool>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    TxnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.CardNum);
                    table.ForeignKey(
                        name: "FK_CreditCard_User_Id",
                        column: x => x.Id,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCard_Transaction_TxnId",
                        column: x => x.TxnId,
                        principalTable: "Transaction",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositCard",
                columns: table => new
                {
                    CardNum = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pin = table.Column<string>(maxLength: 4, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    StartDeposit = table.Column<DateTime>(nullable: false),
                    Commission = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    TxnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositCard", x => x.CardNum);
                    table.ForeignKey(
                        name: "FK_DepositCard_User_Id",
                        column: x => x.Id,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositCard_Transaction_TxnId",
                        column: x => x.TxnId,
                        principalTable: "Transaction",
                        principalColumn: "TxnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckingCard_Id",
                table: "CheckingCard",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckingCard_TxnId",
                table: "CheckingCard",
                column: "TxnId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_Id",
                table: "CreditCard",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_TxnId",
                table: "CreditCard",
                column: "TxnId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCard_Id",
                table: "DepositCard",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCard_TxnId",
                table: "DepositCard",
                column: "TxnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckingCard");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropTable(
                name: "DepositCard");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
