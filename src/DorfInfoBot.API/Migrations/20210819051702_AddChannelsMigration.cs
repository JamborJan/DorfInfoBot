using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DorfInfoBot.API.Migrations
{
    public partial class AddChannelsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Broadcast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NewsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOfBroadcast = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Broadcast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Broadcast_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Broadcast_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Channel",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Telegram" });

            migrationBuilder.InsertData(
                table: "Broadcast",
                columns: new[] { "Id", "ChannelId", "DateOfBroadcast", "NewsId" },
                values: new object[] { 1, 1, new DateTime(1981, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Broadcast",
                columns: new[] { "Id", "ChannelId", "DateOfBroadcast", "NewsId" },
                values: new object[] { 2, 1, new DateTime(1983, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Broadcast_ChannelId",
                table: "Broadcast",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Broadcast_NewsId",
                table: "Broadcast",
                column: "NewsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Broadcast");

            migrationBuilder.DropTable(
                name: "Channel");
        }
    }
}
