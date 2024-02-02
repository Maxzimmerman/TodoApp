using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedApplicationUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_todos_Todos",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Todos",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Todos",
                table: "AspNetUsers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Todos",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Todos",
                table: "AspNetUsers",
                column: "Todos");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_todos_Todos",
                table: "AspNetUsers",
                column: "Todos",
                principalTable: "todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
