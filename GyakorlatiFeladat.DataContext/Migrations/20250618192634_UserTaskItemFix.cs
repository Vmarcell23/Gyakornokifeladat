using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class UserTaskItemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tasks_TaskItemId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TaskItemId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TaskItemId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "TaskItemUser",
                columns: table => new
                {
                    TaskItemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemUser", x => new { x.TaskItemsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Tasks_TaskItemsId",
                        column: x => x.TaskItemsId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemUser_UsersId",
                table: "TaskItemUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemUser");

            migrationBuilder.AddColumn<int>(
                name: "TaskItemId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TaskItemId",
                table: "Users",
                column: "TaskItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tasks_TaskItemId",
                table: "Users",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
