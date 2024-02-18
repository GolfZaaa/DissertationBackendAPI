using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a891df9-5564-4085-b59c-6990d1697c25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c17075b-d463-40b9-a79e-fda949f13c20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "237a4478-29a6-4ea1-8711-44924d26072a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5599ceb3-1800-44b2-b413-c3e9960529fe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db10f34e-a751-4054-aceb-904cbcb143b6");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "WalkInTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "WalkInMemberships",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "WalkInTransactions");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "WalkInMemberships");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a891df9-5564-4085-b59c-6990d1697c25", "นักศึกษา", "Student", "Student" },
                    { "1c17075b-d463-40b9-a79e-fda949f13c20", "ผู้ดูแลระบบ", "Administrator", "Administrator" },
                    { "237a4478-29a6-4ea1-8711-44924d26072a", "บุคคลภายนอก", "Outsider", "Outsider" },
                    { "5599ceb3-1800-44b2-b413-c3e9960529fe", "ผู้อนุมัติ", "Approver", "Approver" },
                    { "db10f34e-a751-4054-aceb-904cbcb143b6", "อาจารย์", "Professor", "Professor" }
                });
        }
    }
}
