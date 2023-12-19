using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class i12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "StatusFinished",
                table: "ReservationsOrders");

            migrationBuilder.AddColumn<int>(
                name: "StatusFinished",
                table: "ReservationsOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "StatusFinished",
                table: "ReservationsOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "StatusFinished",
                table: "ReservationsOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
