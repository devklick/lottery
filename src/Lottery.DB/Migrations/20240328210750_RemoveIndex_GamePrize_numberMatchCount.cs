using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIndex_GamePrize_numberMatchCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_game_prize_game_id_number_match_count",
                schema: "dbo",
                table: "game_prize");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "52280e41296c4a5e8174f8784248bc42");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "36cac6ed3d014fdbbc81d05d31271c57");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "8d5defcfbc2b41f4a0b6b688d6b2b3cc");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "2b024b8b4ea8482f9a52b178f8db4f5f");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "2322bfebfc0c431888d562f9ddab64b2", "AQAAAAIAAYagAAAAEELd8ryhW0K/RNt+OWkZMthNnRzoHhZt9UZsmjQw+zsRalRmUnocjXSDo+zS3AFE3Q==", "d6ce8ca4a5aa40e990739387adbf276c" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "14d6d9e59ee8404b9ae5eeb6e4e93649", "AQAAAAIAAYagAAAAEP10n4hvnAc3t7C37OKXmewiCwNxeplBD4bT0rfi3aROpPl3qQahy73GQSsj4S19zA==", "9f55cc840dd248c99227ae8481687016" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "891374d684394e6384b5afdcf21c365d", "734382eaf85944b0a5ba7ba9f270b3af" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "87d73fa701bf494b9ef9c5f193278a3f", "bdd00b91be02492cb91154dabb5ce5a2" });

            migrationBuilder.CreateIndex(
                name: "IX_game_prize_game_id_number_match_count",
                schema: "dbo",
                table: "game_prize",
                columns: new[] { "game_id", "number_match_count" },
                unique: true);
        }
    }
}
