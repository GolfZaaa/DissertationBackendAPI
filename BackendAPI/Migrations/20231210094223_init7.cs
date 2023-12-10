using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "255646cc-9d3f-4544-92b1-2ad026686e87");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4fc93dc5-c675-4410-befe-cd4581b12b0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd9cd019-931b-4802-9bf5-b0df2f69dc82");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec1aed27-b685-4efb-8aa1-0ad8b460c2ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edaf7904-1967-40e6-b5a4-037afef400ed");

            migrationBuilder.CreateTable(
                name: "ReservationCartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationsId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationCartItems_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCartItems_ReservationsId",
                table: "ReservationCartItems",
                column: "ReservationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationCartItems");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "255646cc-9d3f-4544-92b1-2ad026686e87", null, "Student", "student" },
                    { "4fc93dc5-c675-4410-befe-cd4581b12b0b", null, "Administrator", "Administrator" },
                    { "dd9cd019-931b-4802-9bf5-b0df2f69dc82", null, "Approver", "Approver" },
                    { "ec1aed27-b685-4efb-8aa1-0ad8b460c2ad", null, "Professor", "Professor" },
                    { "edaf7904-1967-40e6-b5a4-037afef400ed", null, "Outsider", "Outsider" }
                });
        }
    }
}
