using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn_Game_CloseTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create the new column as nullable
            migrationBuilder.AddColumn<DateTime>(
                name: "close_time",
                schema: "dbo",
                table: "game",
                type: "timestamp with time zone",
                nullable: true);

            // backfill the data
            migrationBuilder.Sql("UPDATE dbo.game SET close_time = draw_time WHERE close_time IS NULL;");

            // modify column to be non-nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "close_time",
                schema: "dbo",
                table: "game",
                type: "timestamp with time zone",
                nullable: false
            );

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "close_time",
                schema: "dbo",
                table: "game");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "d47e5f2794164412b06151f4b64a5777");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "bd8f98ee0be14a51a37594bad8595f0f");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "8ceee55964df45f5b013b56a89061a17");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "b2c45ade21d44113b36f79d7ae9c9d8f");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "49eadda5615740f7a97db1625ececbf4", "AQAAAAIAAYagAAAAEBBHZ1n9Mz+NtpQlqPzXnJUAYcuUx76G1PCke9mFlUpufAJ939XvI04BnyTRWi6wUQ==", "7622236a4e484a3588f71ed6a48e1a0e" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "46b860548cbf49a1b9be4e75690916dd", "AQAAAAIAAYagAAAAEL+/kqMUGSwbEhPvmN/GxgaFQ4XBKJwB3HkaJOtYgPUytA/f4k1Qy1Svikj1t7tZ5w==", "9636ea5f009a4452ab099c1adaa04bbc" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "ab19a6b2941846e49946c27da68055e8", "4fa2e92e048e43cdb99585e7987fe650" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "4d33bd567f44461097644761a3f98c6e", "0ef5a4c4abde49ee86f81f05f848199f" });
        }
    }
}
