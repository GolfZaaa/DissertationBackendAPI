using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "CategoryLocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "CategoryLocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f832602-d53f-4ba8-89a7-ed490d1a9084", null, "Outsider", "Outsider" },
                    { "7aba6b11-6444-419a-9336-fa109f8f9d0c", null, "Administrator", "Administrator" },
                    { "7be398a7-7dab-4c08-8f40-3fc2f59c5600", null, "Professor", "Professor" },
                    { "fa08e235-a12a-48d3-be71-93557c65dbbb", null, "Approver", "Approver" },
                    { "ff83b677-6b66-4737-be2e-6defc8860206", null, "Student", "student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f832602-d53f-4ba8-89a7-ed490d1a9084");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7aba6b11-6444-419a-9336-fa109f8f9d0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7be398a7-7dab-4c08-8f40-3fc2f59c5600");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa08e235-a12a-48d3-be71-93557c65dbbb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff83b677-6b66-4737-be2e-6defc8860206");

            migrationBuilder.DropColumn(
                name: "Detail",
                table: "CategoryLocations");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "CategoryLocations");

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
    }
}
