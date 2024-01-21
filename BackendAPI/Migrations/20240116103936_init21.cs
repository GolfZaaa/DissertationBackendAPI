using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "CountPeople",
                table: "ReservationsOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12fb4027-42d4-4273-92d8-af3e1a53392a", null, "Outsider", "Outsider" },
                    { "44e2b2ae-42d9-44c1-aadb-3c2a9586110e", null, "Administrator", "Administrator" },
                    { "50d3d85c-e39f-4ab4-89d9-ff5d89ba6350", null, "Approver", "Approver" },
                    { "999871f0-e1d8-4859-9232-c570671e5f09", null, "Student", "student" },
                    { "c3980d70-20f2-4c80-9013-d9c920f9af4e", null, "Professor", "Professor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12fb4027-42d4-4273-92d8-af3e1a53392a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44e2b2ae-42d9-44c1-aadb-3c2a9586110e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50d3d85c-e39f-4ab4-89d9-ff5d89ba6350");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "999871f0-e1d8-4859-9232-c570671e5f09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3980d70-20f2-4c80-9013-d9c920f9af4e");

            migrationBuilder.DropColumn(
                name: "CountPeople",
                table: "ReservationsOrderItems");

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
    }
}
