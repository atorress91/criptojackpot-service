using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoJackpotService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserReferral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_identification",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_phone",
                table: "users");

            migrationBuilder.CreateTable(
                name: "user_referral",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    referrer_id = table.Column<long>(type: "bigint", nullable: false),
                    referred_id = table.Column<long>(type: "bigint", nullable: false),
                    used_security_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_referral", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_referral_users_referred_id",
                        column: x => x.referred_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_referral_users_referrer_id",
                        column: x => x.referrer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_referral_referred_id",
                table: "user_referral",
                column: "referred_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_referral_referrer_id",
                table: "user_referral",
                column: "referrer_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_referral_used_security_code",
                table: "user_referral",
                column: "used_security_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_referral");

            migrationBuilder.CreateIndex(
                name: "ix_users_identification",
                table: "users",
                column: "identification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_phone",
                table: "users",
                column: "phone",
                unique: true);
        }
    }
}
