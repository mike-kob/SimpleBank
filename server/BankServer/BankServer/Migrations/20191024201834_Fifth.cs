using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankServer.Migrations
{
    public partial class Fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndLimit",
                table: "CreditCard");

            migrationBuilder.AddColumn<decimal>(
                name: "MinSum",
                table: "CreditCard",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinSum",
                table: "CreditCard");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndLimit",
                table: "CreditCard",
                type: "datetime2",
                nullable: true);
        }
    }
}
