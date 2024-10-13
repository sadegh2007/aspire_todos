using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspireTodo.Todos.Migrations
{
    /// <inheritdoc />
    public partial class FixTodoStateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TodoId",
                table: "TodoStates",
                newName: "Todo_Id");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "TodoStates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Todo_CompletedAt",
                table: "TodoStates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Todo_CreatedAt",
                table: "TodoStates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "Todo_IsCompleted",
                table: "TodoStates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Todo_Summery",
                table: "TodoStates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Todo_Title",
                table: "TodoStates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "TodoStates");

            migrationBuilder.DropColumn(
                name: "Todo_CompletedAt",
                table: "TodoStates");

            migrationBuilder.DropColumn(
                name: "Todo_CreatedAt",
                table: "TodoStates");

            migrationBuilder.DropColumn(
                name: "Todo_IsCompleted",
                table: "TodoStates");

            migrationBuilder.DropColumn(
                name: "Todo_Summery",
                table: "TodoStates");

            migrationBuilder.DropColumn(
                name: "Todo_Title",
                table: "TodoStates");

            migrationBuilder.RenameColumn(
                name: "Todo_Id",
                table: "TodoStates",
                newName: "TodoId");
        }
    }
}
