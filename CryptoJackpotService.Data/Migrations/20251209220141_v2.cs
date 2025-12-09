using System;
using System.Collections.Generic;
using CryptoJackpotService.Data.Database.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoJackpotService.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prize_images_prizes_prize_id1",
                table: "prize_images");

            migrationBuilder.DropForeignKey(
                name: "fk_winners_prize_tiers_prize_tier_id",
                table: "winners");

            migrationBuilder.DropTable(
                name: "prize_tiers");

            migrationBuilder.DropIndex(
                name: "ix_prize_images_prize_id1",
                table: "prize_images");

            migrationBuilder.DropIndex(
                name: "ix_lottery_numbers_lottery_id_number_series",
                table: "lottery_numbers");

            migrationBuilder.DropColumn(
                name: "prize_id1",
                table: "prize_images");

            migrationBuilder.RenameColumn(
                name: "prize_tier_id",
                table: "winners",
                newName: "prize_id");

            migrationBuilder.RenameIndex(
                name: "ix_winners_prize_tier_id",
                table: "winners",
                newName: "ix_winners_prize_id");

            migrationBuilder.AddColumn<List<PrizeImage>>(
                name: "additional_images",
                table: "prizes",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "claimed_at",
                table: "prizes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "lottery_id",
                table: "prizes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "tier",
                table: "prizes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "winner_ticket_id",
                table: "prizes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_prizes_lottery_id",
                table: "prizes",
                column: "lottery_id");

            migrationBuilder.CreateIndex(
                name: "ix_prizes_winner_ticket_id",
                table: "prizes",
                column: "winner_ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_lottery_numbers_lottery_id",
                table: "lottery_numbers",
                column: "lottery_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prizes_lotteries_lottery_id",
                table: "prizes",
                column: "lottery_id",
                principalTable: "lotteries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prizes_tickets_winner_ticket_id",
                table: "prizes",
                column: "winner_ticket_id",
                principalTable: "tickets",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_winners_prizes_prize_id",
                table: "winners",
                column: "prize_id",
                principalTable: "prizes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prizes_lotteries_lottery_id",
                table: "prizes");

            migrationBuilder.DropForeignKey(
                name: "fk_prizes_tickets_winner_ticket_id",
                table: "prizes");

            migrationBuilder.DropForeignKey(
                name: "fk_winners_prizes_prize_id",
                table: "winners");

            migrationBuilder.DropIndex(
                name: "ix_prizes_lottery_id",
                table: "prizes");

            migrationBuilder.DropIndex(
                name: "ix_prizes_winner_ticket_id",
                table: "prizes");

            migrationBuilder.DropIndex(
                name: "ix_lottery_numbers_lottery_id",
                table: "lottery_numbers");

            migrationBuilder.DropColumn(
                name: "additional_images",
                table: "prizes");

            migrationBuilder.DropColumn(
                name: "claimed_at",
                table: "prizes");

            migrationBuilder.DropColumn(
                name: "lottery_id",
                table: "prizes");

            migrationBuilder.DropColumn(
                name: "tier",
                table: "prizes");

            migrationBuilder.DropColumn(
                name: "winner_ticket_id",
                table: "prizes");

            migrationBuilder.RenameColumn(
                name: "prize_id",
                table: "winners",
                newName: "prize_tier_id");

            migrationBuilder.RenameIndex(
                name: "ix_winners_prize_id",
                table: "winners",
                newName: "ix_winners_prize_tier_id");

            migrationBuilder.AddColumn<Guid>(
                name: "prize_id1",
                table: "prize_images",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "prize_tiers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lottery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prize_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    number_of_winners = table.Column<int>(type: "integer", nullable: false),
                    tier = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prize_tiers", x => x.id);
                    table.ForeignKey(
                        name: "fk_prize_tiers_lotteries_lottery_id",
                        column: x => x.lottery_id,
                        principalTable: "lotteries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prize_tiers_prizes_prize_id",
                        column: x => x.prize_id,
                        principalTable: "prizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prize_images_prize_id1",
                table: "prize_images",
                column: "prize_id1");

            migrationBuilder.CreateIndex(
                name: "ix_lottery_numbers_lottery_id_number_series",
                table: "lottery_numbers",
                columns: new[] { "lottery_id", "number", "series" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_prize_tiers_lottery_id",
                table: "prize_tiers",
                column: "lottery_id");

            migrationBuilder.CreateIndex(
                name: "ix_prize_tiers_prize_id",
                table: "prize_tiers",
                column: "prize_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prize_images_prizes_prize_id1",
                table: "prize_images",
                column: "prize_id1",
                principalTable: "prizes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_winners_prize_tiers_prize_tier_id",
                table: "winners",
                column: "prize_tier_id",
                principalTable: "prize_tiers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
