using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStaffFromBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Staffs_StaffId",
                table: "Bills");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Staffs_StaffId",
                table: "Bills",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Staffs_StaffId",
                table: "Bills");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Staffs_StaffId",
                table: "Bills",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
