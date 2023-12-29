using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class ini11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1957e3eb-d728-4531-a6ec-20bed4399b7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2abce86a-2c92-40a6-a27c-ee700e207628");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3a7906b-1b3a-49a2-973c-10d253583d21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0b7d564-d7b0-4fb5-8e17-e3d8e03633f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9afa736-5ae3-4695-8b66-2c216b2b05ba");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52ce4b3b-5111-4a1c-b269-ec557ca55247", null, "Student", "student" },
                    { "78c0802c-a61c-4c2d-a83e-28b914613b49", null, "Professor", "Professor" },
                    { "8a70fb6f-db5e-4cb9-812c-3dbcba073d2c", null, "Outsider", "Outsider" },
                    { "9cb8956d-e97e-47b8-ade4-d9081b257e72", null, "Administrator", "Administrator" },
                    { "af131ee9-248c-4ced-8d94-e7c1dc898a0a", null, "Approver", "Approver" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52ce4b3b-5111-4a1c-b269-ec557ca55247");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78c0802c-a61c-4c2d-a83e-28b914613b49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a70fb6f-db5e-4cb9-812c-3dbcba073d2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cb8956d-e97e-47b8-ade4-d9081b257e72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af131ee9-248c-4ced-8d94-e7c1dc898a0a");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1957e3eb-d728-4531-a6ec-20bed4399b7c", null, "Approver", "Approver" },
                    { "2abce86a-2c92-40a6-a27c-ee700e207628", null, "Student", "student" },
                    { "c3a7906b-1b3a-49a2-973c-10d253583d21", null, "Outsider", "Outsider" },
                    { "d0b7d564-d7b0-4fb5-8e17-e3d8e03633f5", null, "Administrator", "Administrator" },
                    { "d9afa736-5ae3-4695-8b66-2c216b2b05ba", null, "Professor", "Professor" }
                });
        }
    }
}
