using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class initi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCarts_AspNetUsers_UserId",
                table: "ReservationCarts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03772573-43e5-4532-8e06-e756a56f66a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3022a043-1667-4589-9d5e-454d4facf65e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "465d4a08-af8f-4ac6-9e05-116e0b0c673b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7037d6f8-d4f5-45e0-8119-0df8b7dbd550");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9f802ab-ac04-429a-ad44-d11e11c25183");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCarts_AspNetUsers_UserId",
                table: "ReservationCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCarts_AspNetUsers_UserId",
                table: "ReservationCarts");

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
                    { "03772573-43e5-4532-8e06-e756a56f66a1", null, "Administrator", "Administrator" },
                    { "3022a043-1667-4589-9d5e-454d4facf65e", null, "Professor", "Professor" },
                    { "465d4a08-af8f-4ac6-9e05-116e0b0c673b", null, "Approver", "Approver" },
                    { "7037d6f8-d4f5-45e0-8119-0df8b7dbd550", null, "Student", "student" },
                    { "f9f802ab-ac04-429a-ad44-d11e11c25183", null, "Outsider", "Outsider" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCarts_AspNetUsers_UserId",
                table: "ReservationCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
