using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspireTodo.UserManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddTodosCountToUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TodosCount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TodosCount",
                table: "Users");
        }
    }
}
