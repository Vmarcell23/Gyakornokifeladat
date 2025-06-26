using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class MenuRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Menus_MenuId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_MenuId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Recipes");

            migrationBuilder.CreateTable(
                name: "MenuRecipes",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRecipes", x => new { x.MenuId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_MenuRecipes_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuRecipes_RecipeId",
                table: "MenuRecipes",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuRecipes");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Recipes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_MenuId",
                table: "Recipes",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Menus_MenuId",
                table: "Recipes",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
