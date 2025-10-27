using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoJackpotService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addPasswordResetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "password_reset_code_expiration",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password_reset_code_expiration",
                table: "users");
        }
    }
}
