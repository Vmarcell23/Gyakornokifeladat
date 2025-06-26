using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Recipe_and_Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemVote_ShoppingItems_ShoppingItemId",
                table: "ShoppingItemVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingItemVote",
                table: "ShoppingItemVote");

            migrationBuilder.RenameTable(
                name: "ShoppingItemVote",
                newName: "ShoppingItemVotes");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemVote_ShoppingItemId",
                table: "ShoppingItemVotes",
                newName: "IX_ShoppingItemVotes_ShoppingItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingItemVotes",
                table: "ShoppingItemVotes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RecipeIds = table.Column<string>(type: "TEXT", nullable: false),
                    When = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    FamilyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Ingredients = table.Column<string>(type: "TEXT", nullable: true),
                    Instructions = table.Column<string>(type: "TEXT", nullable: true),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    FamilyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_FamilyId",
                table: "Menus",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_FamilyId",
                table: "Recipes",
                column: "FamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemVotes_ShoppingItems_ShoppingItemId",
                table: "ShoppingItemVotes",
                column: "ShoppingItemId",
                principalTable: "ShoppingItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItemVotes_ShoppingItems_ShoppingItemId",
                table: "ShoppingItemVotes");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingItemVotes",
                table: "ShoppingItemVotes");

            migrationBuilder.RenameTable(
                name: "ShoppingItemVotes",
                newName: "ShoppingItemVote");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingItemVotes_ShoppingItemId",
                table: "ShoppingItemVote",
                newName: "IX_ShoppingItemVote_ShoppingItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingItemVote",
                table: "ShoppingItemVote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItemVote_ShoppingItems_ShoppingItemId",
                table: "ShoppingItemVote",
                column: "ShoppingItemId",
                principalTable: "ShoppingItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
