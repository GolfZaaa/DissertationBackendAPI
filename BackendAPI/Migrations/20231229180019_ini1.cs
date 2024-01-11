using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class ini1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52ce4b3b-5111-4a1c-b269-ec557ca55247");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78c0802c-a61c-4c2d-a83e-28b914613b49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a70fb6f-db5e-4cb9-812c-3dbcba073d2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9cb8956d-e97e-47b8-ade4-d9081b257e72");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af131ee9-248c-4ced-8d94-e7c1dc898a0a");

            migrationBuilder.AddColumn<int>(
                name: "AgencyId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52ce4b3b-5111-4a1c-b269-ec557ca55247", null, "Student", "student" },
                    { "78c0802c-a61c-4c2d-a83e-28b914613b49", null, "Professor", "Professor" },
                    { "8a70fb6f-db5e-4cb9-812c-3dbcba073d2c", null, "Outsider", "Outsider" },
                    { "9cb8956d-e97e-47b8-ade4-d9081b257e72", null, "Administrator", "Administrator" },
                    { "af131ee9-248c-4ced-8d94-e7c1dc898a0a", null, "Approver", "Approver" }
                });
        }
    }
}
