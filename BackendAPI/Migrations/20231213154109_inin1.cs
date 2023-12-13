using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class inin1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "072e6b5f-8ec6-49e7-8f0d-f221ed19d94d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ac46287-bb3b-4d2c-ac76-e3d33ce213eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bd23eed-018d-41e8-be15-a5204380ddf4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7dab1c76-9340-4c38-b641-50d4e11f17e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fdba49d-f564-436b-822d-4e797c3a7836");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "ReservationsOrders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5972b758-c204-41b3-b104-358e266060f6", null, "Outsider", "Outsider" },
                    { "6c2c7a2a-842c-4153-ad52-706c8201c9e6", null, "Approver", "Approver" },
                    { "769527f4-a2d6-4ce8-8bb4-5a31f2682c61", null, "Student", "student" },
                    { "c89818ac-4009-4bbf-b9ef-a1f6b7be41dc", null, "Professor", "Professor" },
                    { "ff1fdc5f-a415-4d85-9c58-cd0faa04607e", null, "Administrator", "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5972b758-c204-41b3-b104-358e266060f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c2c7a2a-842c-4153-ad52-706c8201c9e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "769527f4-a2d6-4ce8-8bb4-5a31f2682c61");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c89818ac-4009-4bbf-b9ef-a1f6b7be41dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff1fdc5f-a415-4d85-9c58-cd0faa04607e");

            migrationBuilder.AddColumn<long>(
                name: "TotalAmount",
                table: "ReservationsOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "072e6b5f-8ec6-49e7-8f0d-f221ed19d94d", null, "Approver", "Approver" },
                    { "4ac46287-bb3b-4d2c-ac76-e3d33ce213eb", null, "Administrator", "Administrator" },
                    { "7bd23eed-018d-41e8-be15-a5204380ddf4", null, "Student", "student" },
                    { "7dab1c76-9340-4c38-b641-50d4e11f17e6", null, "Outsider", "Outsider" },
                    { "9fdba49d-f564-436b-822d-4e797c3a7836", null, "Professor", "Professor" }
                });
        }
    }
}
