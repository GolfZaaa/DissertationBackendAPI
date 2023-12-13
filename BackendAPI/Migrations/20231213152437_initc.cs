using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class initc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "171b88f2-b27c-45e8-8636-1d6172c94667");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b51f6bf-f11f-48a0-bae0-732cfd1da714");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "943e981e-8f97-4541-8379-2cdd4ab9f8e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b81cf213-c94a-4940-8a63-9752879f80b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7ee643f-d0ae-4761-a215-f9f86f0b29ad");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "ReservationsOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "OrderStatus",
                table: "ReservationsOrders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "171b88f2-b27c-45e8-8636-1d6172c94667", null, "Outsider", "Outsider" },
                    { "2b51f6bf-f11f-48a0-bae0-732cfd1da714", null, "Student", "student" },
                    { "943e981e-8f97-4541-8379-2cdd4ab9f8e6", null, "Administrator", "Administrator" },
                    { "b81cf213-c94a-4940-8a63-9752879f80b5", null, "Professor", "Professor" },
                    { "c7ee643f-d0ae-4761-a215-f9f86f0b29ad", null, "Approver", "Approver" }
                });
        }
    }
}
