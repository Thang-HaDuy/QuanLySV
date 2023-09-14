using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class rename_userid_AppUser_HocSinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoSoHS_Users_UserId",
                table: "HoSoHS");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HoSoHS",
                newName: "HocSinhId");

            migrationBuilder.RenameIndex(
                name: "IX_HoSoHS_UserId",
                table: "HoSoHS",
                newName: "IX_HoSoHS_HocSinhId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoHS_Users_HocSinhId",
                table: "HoSoHS",
                column: "HocSinhId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoSoHS_Users_HocSinhId",
                table: "HoSoHS");

            migrationBuilder.RenameColumn(
                name: "HocSinhId",
                table: "HoSoHS",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HoSoHS_HocSinhId",
                table: "HoSoHS",
                newName: "IX_HoSoHS_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoSoHS_Users_UserId",
                table: "HoSoHS",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
