using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b61d6a67-65a0-4d38-bb1a-2ef9ed01acfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be919a47-f651-460e-9899-db6044117542");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4f834ba-ed6e-4ff2-8e69-5b6d89135769");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5612f46-76a7-4aa6-a752-7023284a0065");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fef35d49-f243-4d1b-a12d-6e6b2a876ca2");

            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16fc424c-b53d-4ba5-a252-765ee98453b8", "อาจารย์", "Professor", "Professor" },
                    { "50c0bf30-f639-4704-a2ab-67c73d262aa2", "ผู้อนุมัติ", "Approver", "Approver" },
                    { "5687ccf2-e52f-40b6-8651-98755ee086b2", "ผู้ดูแลระบบ", "Administrator", "Administrator" },
                    { "d66bdcba-07e4-4630-9716-5a36d7a816b9", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "ebbfd4b3-b632-4a16-828e-e36f4056290c", "นักศึกษา", "Student", "Student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16fc424c-b53d-4ba5-a252-765ee98453b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50c0bf30-f639-4704-a2ab-67c73d262aa2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5687ccf2-e52f-40b6-8651-98755ee086b2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d66bdcba-07e4-4630-9716-5a36d7a816b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebbfd4b3-b632-4a16-828e-e36f4056290c");

            migrationBuilder.DropColumn(
                name: "Selected",
                table: "CartItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b61d6a67-65a0-4d38-bb1a-2ef9ed01acfd", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "be919a47-f651-460e-9899-db6044117542", "ผู้ดูแลระบบ", "Administrator", "Administrator" },
                    { "d4f834ba-ed6e-4ff2-8e69-5b6d89135769", "นักศึกษา", "Student", "Student" },
                    { "f5612f46-76a7-4aa6-a752-7023284a0065", "ผู้อนุมัติ", "Approver", "Approver" },
                    { "fef35d49-f243-4d1b-a12d-6e6b2a876ca2", "อาจารย์", "Professor", "Professor" }
                });
        }
    }
}
