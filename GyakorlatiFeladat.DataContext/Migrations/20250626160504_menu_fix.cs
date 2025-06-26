using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class menu_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeIds",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Recipes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Menus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "RecipeIds",
                table: "Menus",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
