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
                keyValue: "04778f8c-2501-440a-b6ee-df7d3cc8f24a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86403c3a-419f-43f6-8710-0d5133465a7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89dc8044-d1f2-48d4-b73e-7815110f6b6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b27d9202-fd1f-4384-aad5-666146e4237f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4328da6-16f3-43f4-ba58-b1c8880b6010");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3623def2-e8b3-433e-b0b6-cd94123be89f", null, "Approver", "Approver" },
                    { "4392ce17-e96d-4374-b846-1bc06c22a278", null, "Outsider", "Outsider" },
                    { "5aa5998d-22de-420d-bd5c-c377fdd64cc6", null, "Professor", "Professor" },
                    { "85a9e1f5-572e-4767-bd14-4bf94be71fec", null, "Administrator", "Administrator" },
                    { "99d67085-4d88-4b8f-a136-3bb07c3bde17", null, "Student", "student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3623def2-e8b3-433e-b0b6-cd94123be89f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4392ce17-e96d-4374-b846-1bc06c22a278");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5aa5998d-22de-420d-bd5c-c377fdd64cc6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85a9e1f5-572e-4767-bd14-4bf94be71fec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99d67085-4d88-4b8f-a136-3bb07c3bde17");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04778f8c-2501-440a-b6ee-df7d3cc8f24a", null, "Outsider", "Outsider" },
                    { "86403c3a-419f-43f6-8710-0d5133465a7c", null, "Student", "student" },
                    { "89dc8044-d1f2-48d4-b73e-7815110f6b6d", null, "Professor", "Professor" },
                    { "b27d9202-fd1f-4384-aad5-666146e4237f", null, "Approver", "Approver" },
                    { "c4328da6-16f3-43f4-ba58-b1c8880b6010", null, "Administrator", "Administrator" }
                });
        }
    }
}
