using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init88 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "010bf3a7-0d7c-409a-b7ff-6a7263b3d2c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "331cdc2f-036d-462e-bb60-a14c714b74da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3528b0e2-4b01-4f7a-a0d9-e3e6364da3ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56ce8b64-e198-4151-9ebe-26bc1d32e49d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e07e76f4-c2cb-4c39-8747-20f99caab644");

            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "ReservationsOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "ReservationsOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalPrice",
                table: "ReservationsOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09a34cf4-8875-4dff-a99e-f59c51fc5b59", null, "Administrator", "Administrator" },
                    { "331b56d0-f7b4-478c-8ef0-c9d69dd7463b", null, "Student", "student" },
                    { "350f6081-074d-4ff6-81ea-79789c0abbe8", null, "Approver", "Approver" },
                    { "65051153-e22e-418b-b206-b1b56e654923", null, "Outsider", "Outsider" },
                    { "e07cce52-5858-4f79-a839-f69ae39681aa", null, "Professor", "Professor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09a34cf4-8875-4dff-a99e-f59c51fc5b59");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "331b56d0-f7b4-478c-8ef0-c9d69dd7463b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "350f6081-074d-4ff6-81ea-79789c0abbe8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65051153-e22e-418b-b206-b1b56e654923");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e07cce52-5858-4f79-a839-f69ae39681aa");

            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "ReservationsOrders");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "ReservationsOrders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ReservationsOrders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "010bf3a7-0d7c-409a-b7ff-6a7263b3d2c8", null, "Professor", "Professor" },
                    { "331cdc2f-036d-462e-bb60-a14c714b74da", null, "Outsider", "Outsider" },
                    { "3528b0e2-4b01-4f7a-a0d9-e3e6364da3ed", null, "Approver", "Approver" },
                    { "56ce8b64-e198-4151-9ebe-26bc1d32e49d", null, "Student", "student" },
                    { "e07e76f4-c2cb-4c39-8747-20f99caab644", null, "Administrator", "Administrator" }
                });
        }
    }
}
