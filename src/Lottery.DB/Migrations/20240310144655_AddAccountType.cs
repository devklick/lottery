using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:account_type", "user,service")
                .Annotation("Npgsql:Enum:item_state", "enabled,disabled")
                .OldAnnotation("Npgsql:Enum:item_state", "enabled,disabled");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "803920fd43da42c78bda56db390f14a0");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "11f0d61b27fb4467b80f0593022ec3d3");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "ca4b070151d24377ade3e59fc31a8aba");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "4fa7a9760d914e59a470510eff1ed1b5");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "4f5589500fd547f69b304f220b4ad223", "AQAAAAIAAYagAAAAEMzR/KJvF8dOoLECoiTDkzM7OtENrGT3XhDybPtCLdHxZlhUQ34Woqf+Q43eqfyYHw==", "02dbf616f05048109173252d03071a08" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "11e924a1dd894268a3b2c25689b470e0", "AQAAAAIAAYagAAAAEKhiHKOjftJfwvDS3YkKkO3mqUR+/B2ecEGQePZBA+MI3N7ls7OeZqSa9syut7BH9A==", "dac23683af504fc6a9fb69f75b80b45a" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "dd9706d644124de7a0a5d6956dc25e23", "AQAAAAIAAYagAAAAEJGgnwB1eNmJHh2jXpxy4ifm01z0XjU5sDi7cd1+QOGZiA1yRW2NeSWx9VzCZpvj2w==", "bc9bbab5df6343279a260d207d7540a8" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "2697558ce0574273b3180cd02d8deb1d", "AQAAAAIAAYagAAAAEHIf5aT8J1FYxdnaBoLZG/RwT8elXkZK4iESGvTQ6visxjaEIvFz3NLLCkzmPpXCbw==", "362675cb5f0f42aba3687ac24fd5efb2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:item_state", "enabled,disabled")
                .OldAnnotation("Npgsql:Enum:account_type", "user,service")
                .OldAnnotation("Npgsql:Enum:item_state", "enabled,disabled");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("19b7d67e-1ad8-4407-b627-d5f56534952f"),
                column: "concurrency_stamp",
                value: "c749d73fc9084bfaacadd2c1f61398b3");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
                column: "concurrency_stamp",
                value: "fc41db8bdd334000bdc34a71a709be12");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
                column: "concurrency_stamp",
                value: "c1a6a455fd8d4d32824dc26154ce5b1a");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_role",
                keyColumn: "id",
                keyValue: new Guid("db16d273-ae17-4822-bbf8-120cec7e3a58"),
                column: "concurrency_stamp",
                value: "305421f008934d40ac58918e07d02940");

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "d871fdcc56d94382b9307af1d1f46622", "AQAAAAIAAYagAAAAENJyJpno3G4VODbS5OR89NiJiGE1NN6qj4wrkn3zU0+xVSdILP3H3IgY4UeMts44tg==", "d6a1a01e707240b8acf890705d4c5971" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("5621cc59-6211-42d2-a4e3-e9584c248adb"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "8f93c04d23d04c9390c2d05e4841d468", "AQAAAAIAAYagAAAAEKMNN4i24bvQHw0RhfzkB2sxJkUp6cfQTwa08XVbeEEilYAvF01l0t9IsKd7zt/M1A==", "dae0262e80e74cde857c800ed49cfa6d" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("a3564302-1a9e-4917-8a48-1a70f211279e"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "33e73b255d734a9090b38e1b40e7ecee", "AQAAAAIAAYagAAAAEI5lgcW7acj+W4TvasMtgOE6RvuoYo4C06VMeFb0OPjiAwHqV/3WQ2T4fjHqAuNuAA==", "85c606b88ac44364a5749c6cc9c8098a" });

            migrationBuilder.UpdateData(
                schema: "idt",
                table: "app_user",
                keyColumn: "id",
                keyValue: new Guid("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
                columns: new[] { "concurrency_stamp", "password_hash", "security_stamp" },
                values: new object[] { "20360c610b3748d4bcb2aa486e1dc804", "AQAAAAIAAYagAAAAEJ0poeQdzdnBNpGOijcdX47SrbjciirhWgwJolNhL/W9lo6wfTUyJoCZHyC22uSfRg==", "26fd177830284bbab80aeae8f8179936" });
        }
    }
}
