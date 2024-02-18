using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32c721ab-1199-4d1e-ae95-8c04ff389470");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f669ccd-3899-41a7-84e5-ab4f8ce9efe2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69e3258c-f7ee-43e9-8421-a425dfbefdab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b093269f-79e0-4671-8617-fcd42bbcbeb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbf0da58-5e59-4fc3-b065-08a512e88a9a");

            migrationBuilder.CreateTable(
                name: "MembershipPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipPrices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22e04da6-9c0d-4474-bf4d-6e948bef2cab", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "2768673d-9e20-4451-9349-003af7259740", "นักศึกษา", "Student", "Student" },
                    { "84fbdaa5-01b7-430a-b478-3f1327147787", "ผู้อนุมัติ", "Approver", "Approver" },
                    { "86620432-18da-4579-9a85-310ae00377e8", "อาจารย์", "Professor", "Professor" },
                    { "bba4d0da-7072-4c7c-80cd-8a761e0ae99f", "ผู้ดูแลระบบ", "Administrator", "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPrices_LocationId",
                table: "MembershipPrices",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembershipPrices");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22e04da6-9c0d-4474-bf4d-6e948bef2cab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2768673d-9e20-4451-9349-003af7259740");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84fbdaa5-01b7-430a-b478-3f1327147787");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86620432-18da-4579-9a85-310ae00377e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bba4d0da-7072-4c7c-80cd-8a761e0ae99f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32c721ab-1199-4d1e-ae95-8c04ff389470", "ผู้ดูแลระบบ", "Administrator", "Administrator" },
                    { "5f669ccd-3899-41a7-84e5-ab4f8ce9efe2", "อาจารย์", "Professor", "Professor" },
                    { "69e3258c-f7ee-43e9-8421-a425dfbefdab", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "b093269f-79e0-4671-8617-fcd42bbcbeb0", "นักศึกษา", "Student", "Student" },
                    { "bbf0da58-5e59-4fc3-b065-08a512e88a9a", "ผู้อนุมัติ", "Approver", "Approver" }
                });
        }
    }
}
