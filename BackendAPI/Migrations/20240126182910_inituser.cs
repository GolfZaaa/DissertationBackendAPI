using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class inituser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01606e76-d337-44f0-91d4-457f0efe5dde");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90c1d9fd-054f-4d47-b478-a891d92c539b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8f4fc04-cdf5-4640-868f-f0182f7e4abf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc100666-c92f-4aeb-9ef5-b06b324214ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc2c52f0-ad64-4f3e-863f-015483057bc2");

            migrationBuilder.AddColumn<int>(
                name: "StatusOnOff",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "41bb4b55-c935-4382-9798-79610e4def12", null, "Approver", "Approver" },
                    { "4801d1e3-6ea5-4f20-a2ab-e00cef6026aa", null, "Student", "student" },
                    { "6e326f1b-b9bc-475d-b227-b9cd23175348", null, "Outsider", "Outsider" },
                    { "8a7eeaa8-20d1-4ddf-a1ea-fbb7edeacc91", null, "Professor", "Professor" },
                    { "f43ef2bd-f247-484d-a033-912eea2bad6a", null, "Administrator", "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41bb4b55-c935-4382-9798-79610e4def12");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4801d1e3-6ea5-4f20-a2ab-e00cef6026aa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e326f1b-b9bc-475d-b227-b9cd23175348");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a7eeaa8-20d1-4ddf-a1ea-fbb7edeacc91");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f43ef2bd-f247-484d-a033-912eea2bad6a");

            migrationBuilder.DropColumn(
                name: "StatusOnOff",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01606e76-d337-44f0-91d4-457f0efe5dde", null, "Student", "student" },
                    { "90c1d9fd-054f-4d47-b478-a891d92c539b", null, "Approver", "Approver" },
                    { "c8f4fc04-cdf5-4640-868f-f0182f7e4abf", null, "Professor", "Professor" },
                    { "cc100666-c92f-4aeb-9ef5-b06b324214ae", null, "Outsider", "Outsider" },
                    { "fc2c52f0-ad64-4f3e-863f-015483057bc2", null, "Administrator", "Administrator" }
                });
        }
    }
}
