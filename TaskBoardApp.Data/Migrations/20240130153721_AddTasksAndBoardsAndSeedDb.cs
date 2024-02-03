using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class AddTasksAndBoardsAndSeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Open" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "In Progress" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Done" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "BoardId", "CreatedOn", "Description", "OwnerId", "Title" },
                values: new object[,]
                {
                    { new Guid("416ea728-3214-4e84-a097-809d8dc4e02a"), 3, new DateTime(2023, 1, 30, 17, 37, 21, 516, DateTimeKind.Local).AddTicks(2865), "Implement [Create Task] page for adding new tasks", "e3751391-c68a-4753-8bc7-9461604e3032", "Create Tasks" },
                    { new Guid("b0db2d92-5666-49dd-b489-f279c9f6269e"), 1, new DateTime(2023, 7, 14, 17, 37, 21, 516, DateTimeKind.Local).AddTicks(2811), "Implement better styling for all public pages", "55a451d4-bdb8-42d2-817d-aea8cce4240c", "Improve CSS styles" },
                    { new Guid("d3af7df2-4e8f-41a7-b290-d4e5977f7ce4"), 2, new DateTime(2023, 12, 30, 17, 37, 21, 516, DateTimeKind.Local).AddTicks(2862), "Create Windows Forms desktop app client for the TaskBoard RESTful API", "e3751391-c68a-4753-8bc7-9461604e3032", "Desktop Client App" },
                    { new Guid("e37b6a6d-1de2-4ba4-9cf9-93c24e87e085"), 1, new DateTime(2023, 8, 30, 17, 37, 21, 516, DateTimeKind.Local).AddTicks(2854), "Create Android client app for the TaskBoard RESTful API", "55a451d4-bdb8-42d2-817d-aea8cce4240c", "Android Client App" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
