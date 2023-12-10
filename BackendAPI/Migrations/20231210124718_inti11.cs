using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class inti11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReservationCarts",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCarts_UserId",
                table: "ReservationCarts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCarts_AspNetUsers_UserId",
                table: "ReservationCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCarts_AspNetUsers_UserId",
                table: "ReservationCarts");

            migrationBuilder.DropIndex(
                name: "IX_ReservationCarts_UserId",
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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReservationCarts");

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
        }
    }
}
