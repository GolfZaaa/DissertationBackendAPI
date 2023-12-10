using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47206def-6aaa-4edc-a633-c328ba85574d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93d2b19b-f9d2-4f1b-b3d0-db8348277fb6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0572dd2-f59a-4072-8518-b55baccef1da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d103b701-a347-40fb-805a-500da04eb49b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5b34fc0-6819-49de-a9d7-804e4e16c248");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47206def-6aaa-4edc-a633-c328ba85574d", null, "Professor", "Professor" },
                    { "93d2b19b-f9d2-4f1b-b3d0-db8348277fb6", null, "Administrator", "Administrator" },
                    { "b0572dd2-f59a-4072-8518-b55baccef1da", null, "Student", "student" },
                    { "d103b701-a347-40fb-805a-500da04eb49b", null, "Outsider", "Outsider" },
                    { "d5b34fc0-6819-49de-a9d7-804e4e16c248", null, "Approver", "Approver" }
                });
        }
    }
}
