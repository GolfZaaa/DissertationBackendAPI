using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "666a8f3e-b3db-4838-936d-62f7930a0535");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d8b2484-d93f-4f4a-93bd-7ab505475e46");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "725dfbc2-ded8-45b0-a0c0-756f15a6c2ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84cfaf5f-29d3-4749-aed5-389fb1ebf4e1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fff96334-5973-4278-98a3-8b203eb1ebe8");

            migrationBuilder.CreateTable(
                name: "locationImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locationImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_locationImages_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "559fda30-8e4e-438a-937b-4f4bf540b53e", null, "Approver", "Approver" },
                    { "7c1ff502-3a54-47dc-b409-e40400599b07", null, "Professor", "Professor" },
                    { "906494a0-3798-4269-bca0-05d8bcd945c7", null, "Administrator", "Administrator" },
                    { "d80139ec-5d3e-4f70-b038-b99e822146d5", null, "Outsider", "Outsider" },
                    { "e830eb70-d545-4cb0-b76f-2fd14ccc8523", null, "Student", "student" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_locationImages_LocationId",
                table: "locationImages",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locationImages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "559fda30-8e4e-438a-937b-4f4bf540b53e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c1ff502-3a54-47dc-b409-e40400599b07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "906494a0-3798-4269-bca0-05d8bcd945c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d80139ec-5d3e-4f70-b038-b99e822146d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e830eb70-d545-4cb0-b76f-2fd14ccc8523");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "666a8f3e-b3db-4838-936d-62f7930a0535", null, "Student", "student" },
                    { "6d8b2484-d93f-4f4a-93bd-7ab505475e46", null, "Approver", "Approver" },
                    { "725dfbc2-ded8-45b0-a0c0-756f15a6c2ed", null, "Professor", "Professor" },
                    { "84cfaf5f-29d3-4749-aed5-389fb1ebf4e1", null, "Administrator", "Administrator" },
                    { "fff96334-5973-4278-98a3-8b203eb1ebe8", null, "Outsider", "Outsider" }
                });
        }
    }
}
