using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numbers_required",
                schema: "dbo",
                table: "game",
                newName: "selections_required_for_entry");

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
                columns: new[] { "concurrency_stamp", "email", "normalized_email", "normalized_user_name", "security_stamp", "user_name" },
                values: new object[] { "61a869272a724d8792ec10b34eca929b", "Lottery.Api.User@Lottery.Game", "LOTTERY.API.USER@LOTTERY.GAME", "LOTTERY.API.USER", "801f9eb0a8cf40fc8a8c621d70ffe214", "Lottery.Api.User" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "email", "normalized_email", "normalized_user_name", "security_stamp", "user_name" },
                values: new object[] { "ff8e66ae5f0b4967afb3d72cec2d9b49", "Lottery.ResultService.User@Lottery.Game", "LOTTERY.RESULTSERVICE.USER@LOTTERY.GAME", "LOTTERY.RESULTSERVICE.USER", "c7ac74e99d6c4e7eb785e2954c391025", "Lottery.ResultService.User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "selections_required_for_entry",
                schema: "dbo",
                table: "game",
                newName: "numbers_required");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "ab02ef94ec384cc49049fc2a5c5b3061");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "3cea94e0ff784364b49c9df292555af0");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "ed2350457c1b4c18afc9d13c23ed5a05");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "cf7a8e8a2eef4e13b6dfeeeed3064cda");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "0c057e3ddbb746aa9fd1ad3f9b98488d", "AQAAAAIAAYagAAAAEPkZPfF6vAdFP7EW+fw9L7JDwvGLMYtp2AMuTgnRudV7tBRtVxrBdOAPm2mAOwNtXA==", "d2761ece200c4ead95f643a5227218a5" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "6cd1f1703ac548e2a9295d2568fdc229", "AQAAAAIAAYagAAAAEDwIZYUsECFbX8jyI6oujyQNsZphVSpLCXTaBoW8ta2Td2yhrkBMUQuLx4R8CoRdfw==", "421123fe685b4be1a9b63c6d809583e5" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "email", "normalized_email", "normalized_user_name", "security_stamp", "user_name" },
                values: new object[] { "ff152f04ba8649f196c20b5f090b7659", "Lottery.Api@Lottery.Game", "LOTTERY.API@LOTTERY.GAME", "LOTTERY.API", "bc1b6b3b3982424f8f5e93d05c4eeb44", "Lottery.Api" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "email", "normalized_email", "normalized_user_name", "security_stamp", "user_name" },
                values: new object[] { "b0851a4bec7c489aaf7a37f311a6a946", "Lottery.ResultService@Lottery.Game", "LOTTERY.RESULTSERVICE@LOTTERY.GAME", "LOTTERY.RESULTSERVICE", "d440692d809e4e2f921e06722aed9ef3", "Lottery.ResultService" });
        }
    }
}
