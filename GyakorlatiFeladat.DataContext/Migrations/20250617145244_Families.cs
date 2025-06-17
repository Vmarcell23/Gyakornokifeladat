using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Families : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FamilyId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyId",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShoppingItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "FamilyId",
                table: "ShoppingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminIds = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_FamilyId",
                table: "Users",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FamilyId",
                table: "Tasks",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_FamilyId",
                table: "ShoppingItems",
                column: "FamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_Families_FamilyId",
                table: "ShoppingItems",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Families_FamilyId",
                table: "Tasks",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Families_FamilyId",
                table: "Users",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_Families_FamilyId",
                table: "ShoppingItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Families_FamilyId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Families_FamilyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Users_FamilyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_FamilyId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingItems_FamilyId",
                table: "ShoppingItems");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "ShoppingItems");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShoppingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
