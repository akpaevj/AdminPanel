using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminPanel.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoBases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConnectionType = table.Column<int>(nullable: false),
                    Server = table.Column<string>(nullable: true),
                    InfoBaseName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    IBasesContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InfoBasesLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ListId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBasesLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InfoBaseInfoBasesLists",
                columns: table => new
                {
                    InfoBaseId = table.Column<Guid>(nullable: false),
                    InfoBasesListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBaseInfoBasesLists", x => new { x.InfoBaseId, x.InfoBasesListId });
                    table.ForeignKey(
                        name: "FK_InfoBaseInfoBasesLists_InfoBases_InfoBaseId",
                        column: x => x.InfoBaseId,
                        principalTable: "InfoBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfoBaseInfoBasesLists_InfoBasesLists_InfoBasesListId",
                        column: x => x.InfoBasesListId,
                        principalTable: "InfoBasesLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sid = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SamAccountName = table.Column<string>(nullable: true),
                    InfoBasesListId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_InfoBasesLists_InfoBasesListId",
                        column: x => x.InfoBasesListId,
                        principalTable: "InfoBasesLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoBaseInfoBasesLists_InfoBasesListId",
                table: "InfoBaseInfoBasesLists",
                column: "InfoBasesListId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_InfoBasesListId",
                table: "Users",
                column: "InfoBasesListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoBaseInfoBasesLists");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "InfoBases");

            migrationBuilder.DropTable(
                name: "InfoBasesLists");
        }
    }
}
