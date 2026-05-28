using System;

using Lottery.Common.Helpers;


using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "idt");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:account_type", "user,service")
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
                    account_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
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
                name: "app_user_invite",
                schema: "idt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    token = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    account_type = table.Column<int>(type: "integer", nullable: false),
                    app_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    state_last_updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_invite", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_user_invite_app_user_app_user_id",
                        column: x => x.app_user_id,
                        principalSchema: "idt",
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_user_invite_app_user_created_by_id",
                        column: x => x.created_by_id,
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
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    close_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    draw_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    resulted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    selections_required_for_entry = table.Column<int>(type: "integer", nullable: false),
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
                name: "app_user_invite_role",
                schema: "idt",
                columns: table => new
                {
                    app_user_invite_id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_invite_role", x => new { x.app_user_invite_id, x.app_role_id });
                    table.ForeignKey(
                        name: "FK_app_user_invite_role_app_role_app_role_id",
                        column: x => x.app_role_id,
                        principalSchema: "idt",
                        principalTable: "app_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_app_user_invite_role_app_user_invite_app_user_invite_id",
                        column: x => x.app_user_invite_id,
                        principalSchema: "idt",
                        principalTable: "app_user_invite",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "entry",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    selection_number = table.Column<int>(type: "integer", nullable: false),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
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
                    { new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"), "b10a7e5e8875420a8d90a55e38b11fb0", "Elevated permissions across the entire system.", "System Administrator", "SystemAdministrator", "SYSTEMADMINISTRATOR" },
                    { new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"), "76c43f385c2b405fba3d946f2bcea6b1", "Permission to create and edit any games", "Game Admin", "GameAdmin", "GAMEADMIN" },
                    { new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"), "4cdc4513a4804f46aa2ef1538249c2d1", "Permission to access the site and play games.", "Basic User", "BasicUser", "BASICUSER" },
                    { new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"), "0b1e458292f14381be395229b68a6e3c", "Role to be assumed by user accounts used by backend services.", "Service Account", "ServiceAccount", "SERVICEACCOUNT" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[,]
                {
                    { new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"), 0, "d2761ece200c4ead95f643a5227218a5", "GameAdmin@Lottery.Game", true, false, null, "GAMEADMIN@LOTTERY.GAME", "GAMEADMIN", "AQAAAAEAACcQAAAAELjUDpUY+Ew4tf3+b2aD4PB5dHyOllNrAhl10GpgXC49Qo4Rl1bthnXm/wD1Dry7Qw==", null, false, "0c057e3ddbb746aa9fd1ad3f9b98488d", false, "GameAdmin" },
                    { new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"), 0, "421123fe685b4be1a9b63c6d809583e5", "SystemAdministrator@Lottery.Game", true, false, null, "SYSTEMADMINISTRATOR@LOTTERY.GAME", "SYSTEMADMIN", "AQAAAAEAACcQAAAAEMBfcZEx4+P6j8kjngjP548MXLwVIEC4bSHvrvHZ7CtTNNaHwcofmAy8kHcQ8eT64w==", null, false, "6cd1f1703ac548e2a9295d2568fdc229", false, "SystemAdmin" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user",
                columns: new[] { "id", "access_failed_count", "account_type", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[,]
                {
                    { new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"), 0, 1, "ConcurrencyStamp", "Lottery.Api.User@Lottery.Game", true, false, null, "LOTTERY.API.USER@LOTTERY.GAME", "LOTTERY.API.SERVICE", null, null, false, "801f9eb0a8cf40fc8a8c621d70ffe214", false, "Lottery.Api.Service" },
                    { new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"), 0, 1, "87d73fa701bf494b9ef9c5f193278a3f", "Lottery.ResultService.User@Lottery.Game", true, false, null, "LOTTERY.RESULTSERVICE.USER@LOTTERY.GAME", "LOTTERY.RESULT.SERVICE", null, null, false, "bdd00b91be02492cb91154dabb5ce5a2", false, "Lottery.Result.Service" }
                });

            migrationBuilder.InsertData(
                schema: "idt",
                table: "app_user_role",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"), new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8") },
                    { new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"), new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb") },
                    { new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"), new Guid("a3564302-1a9e-4917-8a48-1a70f211279e") },
                    { new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"), new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818") }
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
                name: "IX_app_user_invite_app_user_id",
                schema: "idt",
                table: "app_user_invite",
                column: "app_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_invite_created_by_id",
                schema: "idt",
                table: "app_user_invite",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_invite_email",
                schema: "idt",
                table: "app_user_invite",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_user_invite_role_app_role_id",
                schema: "idt",
                table: "app_user_invite_role",
                column: "app_role_id");

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
                name: "IX_entry_prize_game_prize_id_entry_id",
                schema: "dbo",
                table: "entry_prize",
                columns: new[] { "game_prize_id", "entry_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entry_selection_created_by_id",
                schema: "dbo",
                table: "entry_selection",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_selection_entry_id_game_selection_id",
                schema: "dbo",
                table: "entry_selection",
                columns: new[] { "entry_id", "game_selection_id" },
                unique: true);

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
                name: "IX_game_prize_game_id_position",
                schema: "dbo",
                table: "game_prize",
                columns: new[] { "game_id", "position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_game_result_created_by_id",
                schema: "dbo",
                table: "game_result",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_result_game_id_selection_id",
                schema: "dbo",
                table: "game_result",
                columns: new[] { "game_id", "selection_id" },
                unique: true);

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
                name: "IX_game_selection_game_id_selection_number",
                schema: "dbo",
                table: "game_selection",
                columns: new[] { "game_id", "selection_number" },
                unique: true);

            CreateUsers(migrationBuilder);
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
                name: "app_user_invite_role",
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
                name: "app_user_invite",
                schema: "idt");

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

        /// <summary>
        /// A bit of a hacky solution to create users that the various services in the stack will use to interact with the DB.
        /// 
        /// This probably shouldnt be done in EF.
        /// 
        /// They should also be removed on Down (to save worrying about idempotency), but never mind...
        /// </summary>
        /// <param name="migrationBuilder"></param>
        private static void CreateUsers(MigrationBuilder migrationBuilder)
        {
            // Create user & role for API
            var apiDBUser = Env.GetRequiredEnvVar("API_DB_USER");
            var apiDBUserRole = $"{apiDBUser}_role";
            IdempotentCreateRole(migrationBuilder, apiDBUserRole, ["SELECT", "INSERT", "UPDATE", "DELETE"], ["dbo", "idt"]);
            IdempotentCreateUser(migrationBuilder, apiDBUser, Env.GetRequiredEnvVar("API_DB_PASSWORD"));
            migrationBuilder.Sql($"GRANT \"{apiDBUserRole}\" TO \"{apiDBUser}\";");

            // Create user & role for result service
            var resultsDBUser = Env.GetRequiredEnvVar("RESULTS_DB_USER");
            var resultsDBUserRole = $"{resultsDBUser}_role";
            IdempotentCreateRole(migrationBuilder, resultsDBUserRole, ["SELECT", "INSERT", "UPDATE", "DELETE"], ["dbo", "idt"]);
            IdempotentCreateUser(migrationBuilder, resultsDBUser, Env.GetRequiredEnvVar("RESULTS_DB_PASSWORD"));
            migrationBuilder.Sql($"GRANT \"{resultsDBUserRole}\" TO \"{resultsDBUser}\";");
        }
        private static void IdempotentCreateRole(MigrationBuilder migrationBuilder, string roleName, string[] privileges, string[] schemas)
        {
            migrationBuilder.Sql($@"
                DO
                $do$
                BEGIN
                IF NOT EXISTS (
                    SELECT FROM pg_catalog.pg_roles
                    WHERE  rolname = '{roleName}')
                    THEN
                        CREATE ROLE ""{roleName}"";
                        GRANT CONNECT ON DATABASE lottery TO ""{roleName}"";
                        GRANT USAGE ON SCHEMA {string.Join(',', schemas)} TO ""{roleName}"";
                        GRANT {string.Join(',', privileges)} ON ALL TABLES IN SCHEMA {string.Join(',', schemas)} TO ""{roleName}"";
                END IF;
                END
                $do$;
            ");
        }
        private static void IdempotentCreateUser(MigrationBuilder migrationBuilder, string username, string password)
        {
            migrationBuilder.Sql($@"
                DO
                $do$
                BEGIN
                IF NOT EXISTS (
                    SELECT FROM pg_catalog.pg_user
                    WHERE  usename = '{username}')
                    THEN
                        CREATE USER ""{username}"" WITH PASSWORD '{password}';
                END IF;
                END
                $do$;
            ");
        }
    }
}
