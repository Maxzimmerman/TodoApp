using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedtodoentry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todos_AspNetUsers_Todos",
                table: "todos");

            migrationBuilder.DropIndex(
                name: "IX_todos_Todos",
                table: "todos");

            migrationBuilder.DropColumn(
                name: "Todos",
                table: "todos");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "todos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "todos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_todos_UserId",
                table: "todos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_todos_AspNetUsers_UserId",
                table: "todos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todos_AspNetUsers_UserId",
                table: "todos");

            migrationBuilder.DropIndex(
                name: "IX_todos_UserId",
                table: "todos");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "todos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "todos");

            migrationBuilder.AddColumn<string>(
                name: "Todos",
                table: "todos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_todos_Todos",
                table: "todos",
                column: "Todos");

            migrationBuilder.AddForeignKey(
                name: "FK_todos_AspNetUsers_Todos",
                table: "todos",
                column: "Todos",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
