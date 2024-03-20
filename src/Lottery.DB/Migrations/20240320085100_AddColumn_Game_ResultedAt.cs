using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn_Game_ResultedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "resulted_at",
                schema: "dbo",
                table: "game",
                type: "timestamp with time zone",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resulted_at",
                schema: "dbo",
                table: "game");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "98f1fb2666f0493b9e71785b4ffa622f");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "3949f85810b9445980462e08ba8415d1");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "466d2622f9e44674b9eabd2ac38ee087");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "30ad9ec7d0d14f84b4a71a586a1f8c7b");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "f3b2180b417349f8948ab988ef75cfc9", "AQAAAAIAAYagAAAAEGpUj0uzb84u59495pWrCMN9ITlhb/mD9eRRzubynS3ivVbVIw6dmGpfLYf6uAVbhw==", "bd43f068b12c4c458db6a972dec446d8" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "781fe85768e04c71b02ca3bb8e826fcc", "AQAAAAIAAYagAAAAEAIwPW6UG4g7DNhP8dgBkQeKaNkrk5PSFp0eFwm0OKY5qqKYhjQ37hIQ1LRSQj0Xjg==", "717f027ced94498291ea28e065651347" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "61a869272a724d8792ec10b34eca929b", "801f9eb0a8cf40fc8a8c621d70ffe214" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "security_stamp" },
                values: new object[] { "ff8e66ae5f0b4967afb3d72cec2d9b49", "c7ac74e99d6c4e7eb785e2954c391025" });
        }
    }
}
