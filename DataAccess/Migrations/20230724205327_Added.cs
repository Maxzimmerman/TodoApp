using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_todos_Todos",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Todos",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Todos",
                table: "AspNetUsers");
        }
    }
}
