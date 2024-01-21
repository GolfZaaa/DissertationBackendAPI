using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2afe6cd6-4fe9-4cee-86f3-1a04cadb7bb9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "403a2de4-e722-4a36-8063-eeba3f6d4242");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e7d64d9-8ace-4a6b-82ec-f256f2410715");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68e04f4f-3795-45b6-8d22-b7f5a51a75f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7dbc7826-8801-4fbc-b0fd-57918fc2935d");

            migrationBuilder.AddColumn<string>(
                name: "Objectives",
                table: "ReservationsOrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3c9b4bb3-ad03-4840-b3d2-6aa5e57e51ad", null, "Approver", "Approver" },
                    { "3d29276b-3bbb-4cec-9ff2-cb7cdb53e590", null, "Administrator", "Administrator" },
                    { "892b0d2d-3b0c-4599-bdbe-c2dac999c411", null, "Student", "student" },
                    { "99d86e16-589e-428a-9c2b-3ddf6324c755", null, "Professor", "Professor" },
                    { "ac122bf3-7e9f-4086-823d-57e1ce2b5783", null, "Outsider", "Outsider" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c9b4bb3-ad03-4840-b3d2-6aa5e57e51ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d29276b-3bbb-4cec-9ff2-cb7cdb53e590");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "892b0d2d-3b0c-4599-bdbe-c2dac999c411");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99d86e16-589e-428a-9c2b-3ddf6324c755");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac122bf3-7e9f-4086-823d-57e1ce2b5783");

            migrationBuilder.DropColumn(
                name: "Objectives",
                table: "ReservationsOrderItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2afe6cd6-4fe9-4cee-86f3-1a04cadb7bb9", null, "Outsider", "Outsider" },
                    { "403a2de4-e722-4a36-8063-eeba3f6d4242", null, "Administrator", "Administrator" },
                    { "5e7d64d9-8ace-4a6b-82ec-f256f2410715", null, "Professor", "Professor" },
                    { "68e04f4f-3795-45b6-8d22-b7f5a51a75f6", null, "Approver", "Approver" },
                    { "7dbc7826-8801-4fbc-b0fd-57918fc2935d", null, "Student", "student" }
                });
        }
    }
}
