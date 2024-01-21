using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class initA1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01606e76-d337-44f0-91d4-457f0efe5dde", null, "Student", "student" },
                    { "90c1d9fd-054f-4d47-b478-a891d92c539b", null, "Approver", "Approver" },
                    { "c8f4fc04-cdf5-4640-868f-f0182f7e4abf", null, "Professor", "Professor" },
                    { "cc100666-c92f-4aeb-9ef5-b06b324214ae", null, "Outsider", "Outsider" },
                    { "fc2c52f0-ad64-4f3e-863f-015483057bc2", null, "Administrator", "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01606e76-d337-44f0-91d4-457f0efe5dde");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90c1d9fd-054f-4d47-b478-a891d92c539b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8f4fc04-cdf5-4640-868f-f0182f7e4abf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc100666-c92f-4aeb-9ef5-b06b324214ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc2c52f0-ad64-4f3e-863f-015483057bc2");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AspNetUsers");

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
    }
}
