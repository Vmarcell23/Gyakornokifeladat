using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GyakorlatiFeladat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class PasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Families_FamilyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TaskItemUser");

            migrationBuilder.DropColumn(
                name: "AdminIds",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Families");

            migrationBuilder.RenameColumn(
                name: "FamilyId",
                table: "Users",
                newName: "TaskItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_FamilyId",
                table: "Users",
                newName: "IX_Users_TaskItemId");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FamilyUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FamilyId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyUsers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyUsers_FamilyId",
                table: "FamilyUsers",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyUsers_UserId",
                table: "FamilyUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tasks_TaskItemId",
                table: "Users",
                column: "TaskItemId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tasks_TaskItemId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "FamilyUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TaskItemId",
                table: "Users",
                newName: "FamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_TaskItemId",
                table: "Users",
                newName: "IX_Users_FamilyId");

            migrationBuilder.AddColumn<string>(
                name: "AdminIds",
                table: "Families",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Families",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TaskItemUser",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemUser", x => new { x.TasksId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Tasks_TasksId",
                        column: x => x.TasksId,
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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Families_FamilyId",
                table: "Users",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id");
        }
    }
}
