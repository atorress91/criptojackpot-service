using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CryptoJackpotService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    iso3 = table.Column<string>(type: "text", maxLength: 3, nullable: true),
                    numeric_code = table.Column<string>(type: "text", maxLength: 3, nullable: true),
                    iso2 = table.Column<string>(type: "text", maxLength: 2, nullable: true),
                    phone_code = table.Column<string>(type: "text", maxLength: 20, nullable: true),
                    capital = table.Column<string>(type: "text", maxLength: 100, nullable: true),
                    currency = table.Column<string>(type: "text", maxLength: 3, nullable: true),
                    currency_name = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    currency_symbol = table.Column<string>(type: "text", maxLength: 5, nullable: true),
                    tld = table.Column<string>(type: "text", maxLength: 10, nullable: true),
                    native = table.Column<string>(type: "text", maxLength: 100, nullable: true),
                    region = table.Column<string>(type: "text", maxLength: 100, nullable: true),
                    subregion = table.Column<string>(type: "text", maxLength: 100, nullable: true),
                    latitude = table.Column<decimal>(type: "numeric(10,8)", nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(11,8)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lotteries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lottery_no = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "text", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 500, nullable: false),
                    min_number = table.Column<int>(type: "integer", nullable: false),
                    max_number = table.Column<int>(type: "integer", nullable: false),
                    total_series = table.Column<int>(type: "integer", nullable: false),
                    ticket_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    max_tickets = table.Column<int>(type: "integer", nullable: false),
                    sold_tickets = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    terms = table.Column<string>(type: "text", nullable: false),
                    has_age_restriction = table.Column<bool>(type: "boolean", nullable: false),
                    minimum_age = table.Column<int>(type: "integer", nullable: true),
                    restricted_countries = table.Column<List<string>>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lotteries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 200, nullable: true),
                    module = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prizes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 500, nullable: false),
                    estimated_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    main_image_url = table.Column<string>(type: "text", maxLength: 500, nullable: false),
                    specifications = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    cash_alternative = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    is_deliverable = table.Column<bool>(type: "boolean", nullable: false),
                    is_digital = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prizes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prize_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    prize_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "text", maxLength: 500, nullable: false),
                    caption = table.Column<string>(type: "text", maxLength: 200, nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    prize_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prize_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_prize_images_prizes_prize_id",
                        column: x => x.prize_id,
                        principalTable: "prizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prize_images_prizes_prize_id1",
                        column: x => x.prize_id1,
                        principalTable: "prizes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "prize_tiers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lottery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prize_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tier = table.Column<int>(type: "integer", nullable: false),
                    number_of_winners = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    permission_id = table.Column<long>(type: "bigint", nullable: false),
                    access_level = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    identification = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    country_id = table.Column<long>(type: "bigint", nullable: false),
                    state_place = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "text", maxLength: 150, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    image_path = table.Column<string>(type: "text", maxLength: 200, nullable: true),
                    google_access_token = table.Column<string>(type: "text", maxLength: 500, nullable: true),
                    google_refresh_token = table.Column<string>(type: "text", maxLength: 500, nullable: true),
                    security_code = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    invoice_number = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    tax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    notes = table.Column<string>(type: "text", maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lottery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    purchase_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    transaction_id = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    is_gift = table.Column<bool>(type: "boolean", nullable: false),
                    gift_recipient_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                    table.ForeignKey(
                        name: "fk_tickets_lotteries_lottery_id",
                        column: x => x.lottery_id,
                        principalTable: "lotteries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tickets_users_gift_recipient_id",
                        column: x => x.gift_recipient_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tickets_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    transaction_number = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    currency = table.Column<string>(type: "text", maxLength: 3, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_method = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    payment_provider = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    provider_transaction_id = table.Column<string>(type: "text", maxLength: 100, nullable: true),
                    error_code = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    error_message = table.Column<string>(type: "text", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invoice_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    tax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_details_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_details_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lottery_numbers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lottery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    series = table.Column<int>(type: "integer", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lottery_numbers", x => x.id);
                    table.ForeignKey(
                        name: "fk_lottery_numbers_lotteries_lottery_id",
                        column: x => x.lottery_id,
                        principalTable: "lotteries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_lottery_numbers_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "winners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lottery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    prize_tier_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    win_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    claim_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivery_address = table.Column<string>(type: "text", maxLength: 500, nullable: false),
                    delivery_status = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    has_selected_cash_alternative = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_winners", x => x.id);
                    table.ForeignKey(
                        name: "fk_winners_lotteries_lottery_id",
                        column: x => x.lottery_id,
                        principalTable: "lotteries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_winners_prize_tiers_prize_tier_id",
                        column: x => x.prize_tier_id,
                        principalTable: "prize_tiers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_winners_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_winners_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_invoice_details_invoice_id",
                table: "invoice_details",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_details_ticket_id",
                table: "invoice_details",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_invoice_number",
                table: "invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoices_user_id",
                table: "invoices",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_lotteries_lottery_no",
                table: "lotteries",
                column: "lottery_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lottery_numbers_lottery_id_number_series",
                table: "lottery_numbers",
                columns: new[] { "lottery_id", "number", "series" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_lottery_numbers_ticket_id",
                table: "lottery_numbers",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_prize_images_prize_id",
                table: "prize_images",
                column: "prize_id");

            migrationBuilder.CreateIndex(
                name: "ix_prize_images_prize_id1",
                table: "prize_images",
                column: "prize_id1");

            migrationBuilder.CreateIndex(
                name: "ix_prize_tiers_lottery_id",
                table: "prize_tiers",
                column: "lottery_id");

            migrationBuilder.CreateIndex(
                name: "ix_prize_tiers_prize_id",
                table: "prize_tiers",
                column: "prize_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_gift_recipient_id",
                table: "tickets",
                column: "gift_recipient_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_lottery_id_user_id_purchase_date",
                table: "tickets",
                columns: new[] { "lottery_id", "user_id", "purchase_date" });

            migrationBuilder.CreateIndex(
                name: "ix_tickets_transaction_id",
                table: "tickets",
                column: "transaction_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tickets_user_id",
                table: "tickets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_invoice_id",
                table: "transactions",
                column: "invoice_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_provider_transaction_id",
                table: "transactions",
                column: "provider_transaction_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_transaction_number",
                table: "transactions",
                column: "transaction_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_country_id",
                table: "users",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_security_code",
                table: "users",
                column: "security_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_winners_lottery_id",
                table: "winners",
                column: "lottery_id");

            migrationBuilder.CreateIndex(
                name: "ix_winners_prize_tier_id",
                table: "winners",
                column: "prize_tier_id");

            migrationBuilder.CreateIndex(
                name: "ix_winners_ticket_id",
                table: "winners",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_winners_user_id",
                table: "winners",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoice_details");

            migrationBuilder.DropTable(
                name: "lottery_numbers");

            migrationBuilder.DropTable(
                name: "prize_images");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "winners");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "prize_tiers");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "prizes");

            migrationBuilder.DropTable(
                name: "lotteries");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
