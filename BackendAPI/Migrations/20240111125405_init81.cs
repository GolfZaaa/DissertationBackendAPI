using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init81 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReservationsOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2afe6cd6-4fe9-4cee-86f3-1a04cadb7bb9", null, "Outsider", "Outsider" },
                    { "403a2de4-e722-4a36-8063-eeba3f6d4242", null, "Administrator", "Administrator" },
                    { "5e7d64d9-8ace-4a6b-82ec-f256f2410715", null, "Professor", "Professor" },
                    { "68e04f4f-3795-45b6-8d22-b7f5a51a75f6", null, "Approver", "Approver" },
                    { "7dbc7826-8801-4fbc-b0fd-57918fc2935d", null, "Student", "student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2afe6cd6-4fe9-4cee-86f3-1a04cadb7bb9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "403a2de4-e722-4a36-8063-eeba3f6d4242");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e7d64d9-8ace-4a6b-82ec-f256f2410715");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68e04f4f-3795-45b6-8d22-b7f5a51a75f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7dbc7826-8801-4fbc-b0fd-57918fc2935d");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReservationsOrders");

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
    }
}
