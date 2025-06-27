using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class MenuVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isNeeded",
                table: "Menus",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MenuVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MenuId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuVotes_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuVotes_MenuId",
                table: "MenuVotes",
                column: "MenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuVotes");

            migrationBuilder.DropColumn(
                name: "isNeeded",
                table: "Menus");
        }
    }
}
