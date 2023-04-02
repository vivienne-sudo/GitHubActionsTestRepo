using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA2V6CP.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayersIdsToTeeTimeBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Player1Id",
                table: "TeeTimeBookings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player2Id",
                table: "TeeTimeBookings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player3Id",
                table: "TeeTimeBookings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Player4Id",
                table: "TeeTimeBookings",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player1Id",
                table: "TeeTimeBookings");

            migrationBuilder.DropColumn(
                name: "Player2Id",
                table: "TeeTimeBookings");

            migrationBuilder.DropColumn(
                name: "Player3Id",
                table: "TeeTimeBookings");

            migrationBuilder.DropColumn(
                name: "Player4Id",
                table: "TeeTimeBookings");
        }
    }
}
