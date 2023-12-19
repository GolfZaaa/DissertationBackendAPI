using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91265da7-1104-45d5-b4df-0734e8e78bb3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9e5bc0e-3a30-483c-91a5-56bfd9534361");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb99cf5c-4a5b-403e-9ebe-8f9c8d8fb82a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e94358bc-1ee0-4dad-9a6f-b16ef0957f42");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc791bb5-28e9-4824-a5d3-48bbefe5d7ad");

            migrationBuilder.AddColumn<string>(
                name: "Objectives",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "364408fb-068e-4843-9324-4bf6e86da9e3", null, "Professor", "Professor" },
                    { "a76e993e-4de6-4aa8-b9cf-ad12940cdf93", null, "Administrator", "Administrator" },
                    { "bb36358b-2c73-415c-8168-54060222f5a4", null, "Outsider", "Outsider" },
                    { "c2738465-70b2-4329-ae2b-6033fe1eceac", null, "Approver", "Approver" },
                    { "e9dbe1fa-3739-4a1e-a53e-ae8bf358a15c", null, "Student", "student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "364408fb-068e-4843-9324-4bf6e86da9e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a76e993e-4de6-4aa8-b9cf-ad12940cdf93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb36358b-2c73-415c-8168-54060222f5a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2738465-70b2-4329-ae2b-6033fe1eceac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9dbe1fa-3739-4a1e-a53e-ae8bf358a15c");

            migrationBuilder.DropColumn(
                name: "Objectives",
                table: "CartItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "91265da7-1104-45d5-b4df-0734e8e78bb3", null, "Student", "student" },
                    { "a9e5bc0e-3a30-483c-91a5-56bfd9534361", null, "Administrator", "Administrator" },
                    { "cb99cf5c-4a5b-403e-9ebe-8f9c8d8fb82a", null, "Approver", "Approver" },
                    { "e94358bc-1ee0-4dad-9a6f-b16ef0957f42", null, "Professor", "Professor" },
                    { "fc791bb5-28e9-4824-a5d3-48bbefe5d7ad", null, "Outsider", "Outsider" }
                });
        }
    }
}
