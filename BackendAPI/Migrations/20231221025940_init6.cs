using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "StatusOnOff",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusOnOff",
                table: "CategoryLocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "StatusOnOff",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "StatusOnOff",
                table: "CategoryLocations");

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
    }
}
