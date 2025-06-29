using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Ingredients_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FamilyInvites_UserId",
                table: "FamilyInvites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyInvites_Users_UserId",
                table: "FamilyInvites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyInvites_Users_UserId",
                table: "FamilyInvites");

            migrationBuilder.DropIndex(
                name: "IX_FamilyInvites_UserId",
                table: "FamilyInvites");
        }
    }
}
