using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "478ff944-476a-40a2-a5e6-4528a2bc8dd3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51331157-c5b2-4131-ba85-642fccf50903");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60adb940-c233-4e0f-b078-9e6980e9262d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0d9150f-ba24-4e90-9b94-74b7928a0b5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb57028b-5e6a-4785-bed6-4e0cef351dd1");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ReservationCartItems");

            migrationBuilder.AddColumn<int>(
                name: "ReservationCartId",
                table: "ReservationCartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalHour",
                table: "ReservationCartItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "ReservationCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationCarts", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCartItems_ReservationCartId",
                table: "ReservationCartItems",
                column: "ReservationCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationCartItems_ReservationCarts_ReservationCartId",
                table: "ReservationCartItems",
                column: "ReservationCartId",
                principalTable: "ReservationCarts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationCartItems_ReservationCarts_ReservationCartId",
                table: "ReservationCartItems");

            migrationBuilder.DropTable(
                name: "ReservationCarts");

            migrationBuilder.DropIndex(
                name: "IX_ReservationCartItems_ReservationCartId",
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

            migrationBuilder.DropColumn(
                name: "ReservationCartId",
                table: "ReservationCartItems");

            migrationBuilder.DropColumn(
                name: "TotalHour",
                table: "ReservationCartItems");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ReservationCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "478ff944-476a-40a2-a5e6-4528a2bc8dd3", null, "Administrator", "Administrator" },
                    { "51331157-c5b2-4131-ba85-642fccf50903", null, "Outsider", "Outsider" },
                    { "60adb940-c233-4e0f-b078-9e6980e9262d", null, "Professor", "Professor" },
                    { "c0d9150f-ba24-4e90-9b94-74b7928a0b5c", null, "Student", "student" },
                    { "eb57028b-5e6a-4785-bed6-4e0cef351dd1", null, "Approver", "Approver" }
                });
        }
    }
}
