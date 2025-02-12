using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b6bf6c9-ae5b-4f19-9676-72aa6112d21b", "6b6bf6c9-ae5b-4f19-9676-72aa6112d21b", "User", "USER" },
                    { "77d9a35c-a1b6-4310-a928-e90de8b23eba", "77d9a35c-a1b6-4310-a928-e90de8b23eba", "Admin", "ADMIN" },
                    { "8e7ce64e-1f00-407b-8ef4-17f826fa01d0", "8e7ce64e-1f00-407b-8ef4-17f826fa01d0", "SuperAdmin", "SUPERADMIN" },
                    { "f0863c80-1ae5-429a-b991-6a7025a6892c", "f0863c80-1ae5-429a-b991-6a7025a6892c", "Doctor", "DOCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b6bf6c9-ae5b-4f19-9676-72aa6112d21b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77d9a35c-a1b6-4310-a928-e90de8b23eba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e7ce64e-1f00-407b-8ef4-17f826fa01d0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0863c80-1ae5-429a-b991-6a7025a6892c");
        }
    }
}
