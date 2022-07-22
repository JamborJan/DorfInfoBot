using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DorfInfoBot.API.Migrations
{
    public partial class NewsDBInitialSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FullText = table.Column<string>(type: "TEXT", nullable: true),
                    DateOriginalPost = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LinkOriginalPost = table.Column<string>(type: "TEXT", nullable: false),
                    ExternalKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PreviewImage = table.Column<string>(type: "TEXT", nullable: true),
                    NewsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "DateOriginalPost", "Description", "ExternalKey", "FullText", "LinkOriginalPost", "Title" },
                values: new object[] { 1, new DateTime(2021, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Details zur Veranstaltung folgen im nächsten \"dierikon informiert.\" und sind auf der Homepage der Gemeinde bereits ab 20. August 2021 aufgeschaltet. Eine Anmeldung wird zwingend notwendig sein.", "InitKey1", "", "https://www.dierikon.ch/aktuellesinformationen/1308194", "Informationsveranstaltung Bahnanlage Dierikon" });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "DateOriginalPost", "Description", "ExternalKey", "FullText", "LinkOriginalPost", "Title" },
                values: new object[] { 2, new DateTime(2021, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Die virtuelle Entdeckungsreise in und um Luzern. Vom 1. bis 31. August 2021 täglich neue Ausflugstipps erhalten und an Gewinnspielen mitmachen.", "InitKey2", "", "https://www.dierikon.ch/aktuellesinformationen/1311797", "Mit der vbl die Stadt Luzern und die Agglomeration entdecken!" });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "DateOriginalPost", "Description", "ExternalKey", "FullText", "LinkOriginalPost", "Title" },
                values: new object[] { 3, new DateTime(2021, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Will einen Pool bauen, so wie alle.", "InitKey3", "", "https://www.dierikon.ch/publikationen/334790", "Baugesuch Hans Mustermann" });

            migrationBuilder.InsertData(
                table: "Attachment",
                columns: new[] { "Id", "NewsId", "PreviewImage", "Title" },
                values: new object[] { 1, 1, "The most visited urban park in the United States.", "Anhang-News-1.pdf" });

            migrationBuilder.InsertData(
                table: "Attachment",
                columns: new[] { "Id", "NewsId", "PreviewImage", "Title" },
                values: new object[] { 2, 1, "A 102-story skyscraper located in Midtown Manhattan.", "Anhang-News-1.pdf" });

            migrationBuilder.InsertData(
                table: "Attachment",
                columns: new[] { "Id", "NewsId", "PreviewImage", "Title" },
                values: new object[] { 3, 2, "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", "Anhang-News-2.pdf" });

            migrationBuilder.InsertData(
                table: "Attachment",
                columns: new[] { "Id", "NewsId", "PreviewImage", "Title" },
                values: new object[] { 4, 2, "The the finest example of railway architecture in Belgium.", "Anhang-News-2.pdf" });

            migrationBuilder.InsertData(
                table: "Attachment",
                columns: new[] { "Id", "NewsId", "PreviewImage", "Title" },
                values: new object[] { 5, 3, "A wrought iron lattice tower on the Champ de Mars, Titled after engineer Gustave Eiffel.", "Anhang-News-3.pdf" });

            migrationBuilder.InsertData(
                table: "Attachment",
                columns: new[] { "Id", "NewsId", "PreviewImage", "Title" },
                values: new object[] { 6, 3, "The world's largest museum.", "Anhang-News-3.pdf" });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_NewsId",
                table: "Attachment",
                column: "NewsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
