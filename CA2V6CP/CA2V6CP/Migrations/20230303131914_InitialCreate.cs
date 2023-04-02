using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CA2V6CP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Golfers",
                columns: table => new
                {
                    MembershipNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Sex = table.Column<string>(type: "TEXT", nullable: false),
                    Handicap = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Golfers", x => x.MembershipNumber);
                });

            migrationBuilder.CreateTable(
                name: "TeeTimeBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Player1Name = table.Column<string>(type: "TEXT", nullable: false),
                    Player1Handicap = table.Column<int>(type: "INTEGER", nullable: false),
                    Player2Name = table.Column<string>(type: "TEXT", nullable: false),
                    Player2Handicap = table.Column<int>(type: "INTEGER", nullable: false),
                    Player3Name = table.Column<string>(type: "TEXT", nullable: false),
                    Player3Handicap = table.Column<int>(type: "INTEGER", nullable: false),
                    Player4Name = table.Column<string>(type: "TEXT", nullable: false),
                    Player4Handicap = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeeTimeBookings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Golfers");

            migrationBuilder.DropTable(
                name: "TeeTimeBookings");
        }
    }
}
