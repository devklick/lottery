using System;

using Lottery.DB.Entities.Ref;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class UserInvite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    state = table.Column<ItemState>(type: "item_state", nullable: false, defaultValue: ItemState.Enabled),
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

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "6e6f494237a14938991da22befc2a8c9");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "3c57c013745c4f83912856bd45ff36f3");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "d2ebf85a88d742ef9f1f8a87a8406207");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "b4b0e8f322c24220a4b82265c8b0e3d4");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "c387af4696b840de8a4914a179f4e56b", "AQAAAAIAAYagAAAAEEkC9O0RSsb50NKSdo5IYa3PZjjXcF2VAvNQQ2wA3UuA0+fQc+FyiXdNhuUFGKMq3A==", "78564cbc858e4cb99cf2aaf5587319ab" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "f12e953db009459fb99491cd24a7aa0d", "AQAAAAIAAYagAAAAECn+QcwPg0RssJpIBCR18ByOI+AoF34BsyndEkJCnNXWXvxLxIgGbPDfKJp5FY+QVg==", "f9ae10d309814c18add087ced0345130" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "d4ad0c6e33ef4a14872c245cf4706333", "b290642e4c004d6384bb8a67d5549759" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "cd05e11bb15f4c169647263d174d253f", "ceaf9bb8f8694076b77a6d38935ccfa0" });

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

            // We've added new tables in idt schema, so we need to give permission to them
            migrationBuilder.Sql($@"
                GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA dbo,idt TO ""Lottery.Api.Role"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_user_invite_role",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "app_user_invite",
                schema: "idt");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "c7168f19bd7c44f6994be12d14ff326c");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "3c3424fbef484ba6b35d4f1fd1f974d2");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "56d13d830ba94182b605bcc9dec66832");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "919cd7ba9abd4f938094b72922b1993a");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "093909605d0d457299f53d235be8a375", "AQAAAAIAAYagAAAAEL+1SczOM9YYpmUTa1uWJKJZuHx4I5i0Nki9MCYrr5yFqQQbXH/LY39xlrjPW3a3Gg==", "d07176427ad9471dae6e58b7af2fa46e" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "6b142fbd00e84a2f9c011994d76bae28", "AQAAAAIAAYagAAAAEHnBhifn3ShqNSdTOrqdqWSe2eE4DEMSEB2gTBtvY+npPFAFQZcY1G3R3RA00bpimA==", "4bdf97d790e347058a9e8a7804efb735" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "2dddf59ef650487a83a6c343f62b1d4c", "8120972cbc4f462098c2aad61e8dcb0a" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "aa5b7cac13c2492a97ee7a8ac59f246f", "02419fb8985742eeac1134163be565d0" });
        }
    }
}
