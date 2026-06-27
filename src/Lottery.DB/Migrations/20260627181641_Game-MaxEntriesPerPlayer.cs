using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lottery.DB.Migrations
{
    /// <inheritdoc />
    public partial class GameMaxEntriesPerPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "max_entries_per_player",
                schema: "dbo",
                table: "game",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_entries_per_player",
                schema: "dbo",
                table: "game");
        }
    }
}
