using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCartItems_Reservations_ReservationsId",
                table: "ReservationCartItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e8e4f4f-196e-45ab-81e1-bf5d42f61774");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e88dc7e-312e-4972-bcaf-8aa56cc56119");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ce26254-12a3-4a48-9ebc-9b815202a51c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c53a8a2-85b4-4e0a-84ca-4920b658dd9b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e75d2370-f585-4e64-84c0-d9cb3e065e6a");

            migrationBuilder.RenameColumn(
                name: "ReservationsId",
                table: "ReservationCartItems",
                newName: "LocationsId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationCartItems_ReservationsId",
                table: "ReservationCartItems",
                newName: "IX_ReservationCartItems_LocationsId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f3e9f50-bd13-4b22-b314-2efca61ddfce", null, "Professor", "Professor" },
                    { "670a1246-bfa1-4de2-884d-28dcbf1fce3a", null, "Outsider", "Outsider" },
                    { "859452f1-cf01-4375-9a79-28ff71224a76", null, "Administrator", "Administrator" },
                    { "880ec371-e7f9-43d3-b1c0-a9d47f2f4f9f", null, "Approver", "Approver" },
                    { "d2442027-fd3c-4db9-9ef2-d0d495cc1535", null, "Student", "student" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCartItems_Locations_LocationsId",
                table: "ReservationCartItems",
                column: "LocationsId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCartItems_Locations_LocationsId",
                table: "ReservationCartItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f3e9f50-bd13-4b22-b314-2efca61ddfce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "670a1246-bfa1-4de2-884d-28dcbf1fce3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "859452f1-cf01-4375-9a79-28ff71224a76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "880ec371-e7f9-43d3-b1c0-a9d47f2f4f9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d2442027-fd3c-4db9-9ef2-d0d495cc1535");

            migrationBuilder.RenameColumn(
                name: "LocationsId",
                table: "ReservationCartItems",
                newName: "ReservationsId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationCartItems_LocationsId",
                table: "ReservationCartItems",
                newName: "IX_ReservationCartItems_ReservationsId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e8e4f4f-196e-45ab-81e1-bf5d42f61774", null, "Approver", "Approver" },
                    { "1e88dc7e-312e-4972-bcaf-8aa56cc56119", null, "Professor", "Professor" },
                    { "5ce26254-12a3-4a48-9ebc-9b815202a51c", null, "Outsider", "Outsider" },
                    { "6c53a8a2-85b4-4e0a-84ca-4920b658dd9b", null, "Administrator", "Administrator" },
                    { "e75d2370-f585-4e64-84c0-d9cb3e065e6a", null, "Student", "student" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCartItems_Reservations_ReservationsId",
                table: "ReservationCartItems",
                column: "ReservationsId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
