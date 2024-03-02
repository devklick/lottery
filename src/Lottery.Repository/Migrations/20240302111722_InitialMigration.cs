using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lottery.Repository.Migrations
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
                name: "entry",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    state_last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    state_last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_role",
                columns: new[] { "id", "concurrency_stamp", "description", "display_name", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("45ed77a2-36f5-44b5-aba2-21835a3110ef"), "a0162189da75467b8f2bfd9e38f9e632", "Permission to access the site and play games.", "Basic User", "BasicUser", "BASICUSER" },
                    { new Guid("488c804e-5321-404e-9c8c-c333f79604c1"), "05857132a7cb44f3922075263e424ded", "Permission to create and edit any games", "Game Admin", "GameAdmin", "GAMEADMIN" },
                    { new Guid("a0738ca5-9f1d-4c34-9829-cd2c389ae31f"), "90c473a2b7f94cda8b281a62b6014020", "Elevated permissions across the entire system.", "System Administrator", "SystemAdministrator", "SYSTEMADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[,]
                {
                    { new Guid("19f40d93-4d51-4458-9e9c-998568aed063"), 0, "43f056d36bc24fc9b559c5440b65ca17", "SystemAdministrator@Lottery.Game", true, false, null, "SYSTEMADMINISTRATOR@LOTTERY.GAME", "SYSTEMADMIN", "AQAAAAIAAYagAAAAEEC+tzoapmfsiLIV7l2aHjFAdFUyOfODeQfgOnrZCmByUYFO/qwokNopTAQGZ84Tmg==", null, false, "5e605c9a4aa043059565de7b250133e7", false, "SystemAdmin" },
                    { new Guid("26fb4004-f122-48eb-9a0d-6df7cda1107e"), 0, "a1b58288e7a24241adbd535a7cede74f", "GameAdmin@Lottery.Game", true, false, null, "GAMEADMIN@LOTTERY.GAME", "GAMEADMIN", "AQAAAAIAAYagAAAAEFTc5+gREzjqz6PActjgXbzorR9yOkq+c/Fg4e4HjrdhOpfXkJSiPasYQBgMUZPWeQ==", null, false, "328a742b02134136aad33c2e63da733f", false, "GameAdmin" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("a0738ca5-9f1d-4c34-9829-cd2c389ae31f"), new Guid("19f40d93-4d51-4458-9e9c-998568aed063") },
                    { new Guid("488c804e-5321-404e-9c8c-c333f79604c1"), new Guid("26fb4004-f122-48eb-9a0d-6df7cda1107e") }
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
                name: "entry_selection",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "app_role",
                schema: "idt");

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
