using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00bb4d14-96e1-4ad0-b1b0-4b0f1382fa39");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0612d3e2-03eb-40ce-b75d-ba32d6244606");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "146abf8c-2f88-42da-8454-f036078e0be7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "382ed985-cfdf-48b7-81e0-5c1c0eca80d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d13bde67-4367-4409-8859-9431fbee8d76");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32c721ab-1199-4d1e-ae95-8c04ff389470", "ผู้ดูแลระบบ", "Administrator", "Administrator" },
                    { "5f669ccd-3899-41a7-84e5-ab4f8ce9efe2", "อาจารย์", "Professor", "Professor" },
                    { "69e3258c-f7ee-43e9-8421-a425dfbefdab", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "b093269f-79e0-4671-8617-fcd42bbcbeb0", "นักศึกษา", "Student", "Student" },
                    { "bbf0da58-5e59-4fc3-b065-08a512e88a9a", "ผู้อนุมัติ", "Approver", "Approver" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32c721ab-1199-4d1e-ae95-8c04ff389470");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f669ccd-3899-41a7-84e5-ab4f8ce9efe2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69e3258c-f7ee-43e9-8421-a425dfbefdab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b093269f-79e0-4671-8617-fcd42bbcbeb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbf0da58-5e59-4fc3-b065-08a512e88a9a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00bb4d14-96e1-4ad0-b1b0-4b0f1382fa39", "ผู้ดูแลระบบ", "Administrator", "Administrator" },
                    { "0612d3e2-03eb-40ce-b75d-ba32d6244606", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "146abf8c-2f88-42da-8454-f036078e0be7", "นักศึกษา", "Student", "Student" },
                    { "382ed985-cfdf-48b7-81e0-5c1c0eca80d1", "อาจารย์", "Professor", "Professor" },
                    { "d13bde67-4367-4409-8859-9431fbee8d76", "ผู้อนุมัติ", "Approver", "Approver" }
                });
        }
    }
}
