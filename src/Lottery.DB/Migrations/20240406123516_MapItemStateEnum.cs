using System;

using Lottery.DB.Entities.Ref;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class MapItemStateEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ChangeTypeToItemState(migrationBuilder, table: "game_selection");

            ChangeTypeToItemState(migrationBuilder, table: "game_result");

            ChangeTypeToItemState(migrationBuilder, table: "game_prize");

            ChangeTypeToItemState(migrationBuilder, table: "game");

            ChangeTypeToItemState(migrationBuilder, table: "entry_selection");

            ChangeTypeToItemState(migrationBuilder, table: "entry_prize");

            ChangeTypeToItemState(migrationBuilder, table: "entry");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ChangeTypeFromItemState(migrationBuilder, table: "game_selection");

            ChangeTypeFromItemState(migrationBuilder, table: "game_result");

            ChangeTypeFromItemState(migrationBuilder, table: "game_prize");

            ChangeTypeFromItemState(migrationBuilder, table: "game");

            ChangeTypeFromItemState(migrationBuilder, table: "entry_selection");

            ChangeTypeFromItemState(migrationBuilder, table: "entry_prize");

            ChangeTypeFromItemState(migrationBuilder, table: "entry");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "cbc4a884296341b98645793d02738f1d");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "6eac1982e9224150ad3f817b95daf8e6");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "f267546ec71d48fcb616f2bebfa49cc3");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "6b5d98df0817489899407079ae662f8a");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "0b01e91d23cb4c9399bb56252b421b0b", "AQAAAAIAAYagAAAAEJMFivgTGOzKTRzRfKRJJbqEUQVpGYkgb05bn7k6wM6aG/6I6kHnbFdhSFb0e4OfVw==", "93339a63d2d74e03b754327335d034ac" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "e2264b6e2129426cb687eb20fff9104b", "AQAAAAIAAYagAAAAEAYgqZpk8IdOxfs/AXFkTX+/BcRDO/hojuB9GQgWpSIfKbBy5Gn+rWyDAlsKudvp1w==", "b397304df76c444d81dbb37550f658ae" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "732960fea7474d30afd6680bdcbc3a42", "48790ec209074d5fb9fe5732092c0a5d" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "4d315608d2ed4d1c97f6c6ad568a39ac", "e95486a3cf6c40e08a8e2d0408b9479b" });
        }

        private void ChangeTypeToItemState(MigrationBuilder migrationBuilder, string table, string column = "state", string schema = "dbo")
        {
            migrationBuilder.AddColumn<ItemState>(
                schema: schema,
                table: table,
                name: "state_temp",
                defaultValue: "enabled"
            );
            migrationBuilder.Sql(@$"
                UPDATE {schema}.{table} 
                SET state_temp = 
                    CASE {column}
                        WHEN 1 THEN 'enabled'::item_state
                        ELSE 'disabled'::item_state
                    END
            ");
            migrationBuilder.DropColumn(
                schema: schema,
                table: table,
                name: column
            );
            migrationBuilder.RenameColumn(
                schema: schema,
                table: table,
                name: "state_temp",
                newName: column
            );
        }

        private void ChangeTypeFromItemState(MigrationBuilder migrationBuilder, string table, string column = "state", string schema = "dbo")
        {
            migrationBuilder.AddColumn<int>(
                schema: schema,
                table: table,
                name: "state_temp",
                defaultValue: 1
            );
            migrationBuilder.Sql(@$"
                UPDATE {schema}.{table} 
                SET state_temp = 
                    CASE {column}
                        WHEN 'enabled' THEN 1
                        ELSE 0
                    END
            ");
            migrationBuilder.DropColumn(
                schema: schema,
                table: table,
                name: column
            );
            migrationBuilder.RenameColumn(
                schema: schema,
                table: table,
                name: "state_temp",
                newName: column
            );
        }
    }
}
