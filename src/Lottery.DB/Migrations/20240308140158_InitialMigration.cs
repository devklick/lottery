using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "idt");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:item_state", "enabled,disabled");

            migrationBuilder.CreateTable(
                name: "app_role",
                schema: "idt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_user",
                schema: "idt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_role_claim",
                schema: "idt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_role_claim_app_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "idt",
                        principalTable: "app_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_user_claim",
                schema: "idt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_user_claim_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_user_login",
                schema: "idt",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "FK_app_user_login_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_user_role",
                schema: "idt",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_app_user_role_app_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "idt",
                        principalTable: "app_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_user_role_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_user_token",
                schema: "idt",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "FK_app_user_token_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "game",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    draw_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    numbers_required = table.Column<int>(type: "integer", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entry",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry", x => x.id);
                    table.ForeignKey(
                        name: "FK_entry_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entry_game_game_id",
                        column: x => x.game_id,
                        principalSchema: "dbo",
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "game_prize",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    number_match_count = table.Column<int>(type: "integer", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_prize", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_prize_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_prize_game_game_id",
                        column: x => x.game_id,
                        principalSchema: "dbo",
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "game_selection",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    selection_number = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_selection", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_selection_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_selection_game_game_id",
                        column: x => x.game_id,
                        principalSchema: "dbo",
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entry_prize",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_prize_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry_prize", x => x.id);
                    table.ForeignKey(
                        name: "FK_entry_prize_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entry_prize_entry_entry_id",
                        column: x => x.entry_id,
                        principalSchema: "dbo",
                        principalTable: "entry",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entry_prize_game_game_id",
                        column: x => x.game_id,
                        principalSchema: "dbo",
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entry_prize_game_prize_game_prize_id",
                        column: x => x.game_prize_id,
                        principalSchema: "dbo",
                        principalTable: "game_prize",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entry_selection",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_selection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry_selection", x => x.id);
                    table.ForeignKey(
                        name: "FK_entry_selection_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entry_selection_entry_entry_id",
                        column: x => x.entry_id,
                        principalSchema: "dbo",
                        principalTable: "entry",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entry_selection_game_selection_game_selection_id",
                        column: x => x.game_selection_id,
                        principalSchema: "dbo",
                        principalTable: "game_selection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "game_result",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    selection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_result", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_result_app_user_created_by_id",
                        column: x => x.created_by_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_result_game_game_id",
                        column: x => x.game_id,
                        principalSchema: "dbo",
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_result_game_selection_selection_id",
                        column: x => x.selection_id,
                        principalSchema: "dbo",
                        principalTable: "game_selection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_role",
                columns: new[] { "id", "concurrency_stamp", "description", "display_name", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("4b794a10-0a07-4221-9460-f97848305470"), "d620fa1c65a4415995cbd3d2da3cf6a1", "Elevated permissions across the entire system.", "System Administrator", "SystemAdministrator", "SYSTEMADMINISTRATOR" },
                    { new Guid("90a235a7-6a26-4798-ae29-a2cd9a7c194c"), "1d1d569fe1f542d6a31dfbc81a83453b", "Permission to access the site and play games.", "Basic User", "BasicUser", "BASICUSER" },
                    { new Guid("f037875c-c8d7-4904-bb18-586b988e87ca"), "0de63a4fbb214a3b9e1722d397cbd803", "Permission to create and edit any games", "Game Admin", "GameAdmin", "GAMEADMIN" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[,]
                {
                    { new Guid("bff4790e-4ba7-4249-83f8-0096b9bc8586"), 0, "6963be86e3de4cc7b4598623eb75dcee", "SystemAdministrator@Lottery.Game", true, false, null, "SYSTEMADMINISTRATOR@LOTTERY.GAME", "SYSTEMADMIN", "AQAAAAIAAYagAAAAEEIHJHV9Ayep4mG5kMkAkCgv7BUrVPdz3ki3ZtxFQF+M6ZEBrc3CBdyLQBKztCkVNg==", null, false, "a8441c0bbc4441219baaac0cf0ad38ec", false, "SystemAdmin" },
                    { new Guid("faef0c72-131b-4578-815e-006701feef64"), 0, "ee5b9fd92abf4c2c90fe1d9133cacce0", "GameAdmin@Lottery.Game", true, false, null, "GAMEADMIN@LOTTERY.GAME", "GAMEADMIN", "AQAAAAIAAYagAAAAELsG6gwr5IdFDuVCa2y4MtDtVQ7qhvcSgLaHArakcssEH9xYRGHvKXEc5qrOpczx2g==", null, false, "2921e8a98ce049f28ec21a1e0c375148", false, "GameAdmin" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("4b794a10-0a07-4221-9460-f97848305470"), new Guid("bff4790e-4ba7-4249-83f8-0096b9bc8586") },
                    { new Guid("f037875c-c8d7-4904-bb18-586b988e87ca"), new Guid("faef0c72-131b-4578-815e-006701feef64") }
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "idt",
                table: "app_role",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_role_claim_role_id",
                schema: "idt",
                table: "app_role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "idt",
                table: "app_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "idt",
                table: "app_user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_user_claim_user_id",
                schema: "idt",
                table: "app_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_login_user_id",
                schema: "idt",
                table: "app_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_role_role_id",
                schema: "idt",
                table: "app_user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_created_by_id",
                schema: "dbo",
                table: "entry",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_game_id",
                schema: "dbo",
                table: "entry",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_prize_created_by_id",
                schema: "dbo",
                table: "entry_prize",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_prize_entry_id",
                schema: "dbo",
                table: "entry_prize",
                column: "entry_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entry_prize_game_id",
                schema: "dbo",
                table: "entry_prize",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_prize_game_prize_id",
                schema: "dbo",
                table: "entry_prize",
                column: "game_prize_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_selection_created_by_id",
                schema: "dbo",
                table: "entry_selection",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_selection_entry_id",
                schema: "dbo",
                table: "entry_selection",
                column: "entry_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_selection_game_selection_id",
                schema: "dbo",
                table: "entry_selection",
                column: "game_selection_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_created_by_id",
                schema: "dbo",
                table: "game",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_prize_created_by_id",
                schema: "dbo",
                table: "game_prize",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_prize_game_id",
                schema: "dbo",
                table: "game_prize",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_result_created_by_id",
                schema: "dbo",
                table: "game_result",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_result_game_id",
                schema: "dbo",
                table: "game_result",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_result_selection_id",
                schema: "dbo",
                table: "game_result",
                column: "selection_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_selection_created_by_id",
                schema: "dbo",
                table: "game_selection",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_selection_game_id",
                schema: "dbo",
                table: "game_selection",
                column: "game_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_role_claim",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "app_user_claim",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "app_user_login",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "app_user_role",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "app_user_token",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "entry_prize",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entry_selection",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "game_result",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "app_role",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "game_prize",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "entry",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "game_selection",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "game",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "app_user",
                schema: "idt");
        }
    }
}
