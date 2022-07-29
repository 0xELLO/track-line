using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeadList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultTitle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CratedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    CratedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeadList_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultTitle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    TotalLength = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCreatedByUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    CratedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    CratedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HeadListId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DefaultTitle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    CratedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    CratedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubList_HeadList_HeadListId",
                        column: x => x.HeadListId,
                        principalTable: "HeadList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserListItemProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ListItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ListObjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    TimesFinished = table.Column<int>(type: "INTEGER", nullable: false),
                    CratedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    CratedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserListItemProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserListItemProgress_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserListItemProgress_ListItem_ListItemId",
                        column: x => x.ListItemId,
                        principalTable: "ListItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListItemInSubList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubListId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ListItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ListObjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    CratedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    CratedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItemInSubList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListItemInSubList_ListItem_ListItemId",
                        column: x => x.ListItemId,
                        principalTable: "ListItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListItemInSubList_SubList_SubListId",
                        column: x => x.SubListId,
                        principalTable: "SubList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeadList_AppUserId",
                table: "HeadList",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListItemInSubList_ListItemId",
                table: "ListItemInSubList",
                column: "ListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ListItemInSubList_SubListId",
                table: "ListItemInSubList",
                column: "SubListId");

            migrationBuilder.CreateIndex(
                name: "IX_SubList_HeadListId",
                table: "SubList",
                column: "HeadListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListItemProgress_AppUserId",
                table: "UserListItemProgress",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserListItemProgress_ListItemId",
                table: "UserListItemProgress",
                column: "ListItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListItemInSubList");

            migrationBuilder.DropTable(
                name: "UserListItemProgress");

            migrationBuilder.DropTable(
                name: "SubList");

            migrationBuilder.DropTable(
                name: "ListItem");

            migrationBuilder.DropTable(
                name: "HeadList");
        }
    }
}
