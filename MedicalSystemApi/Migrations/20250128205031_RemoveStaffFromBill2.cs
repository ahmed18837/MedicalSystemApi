using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStaffFromBill2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Staffs_StaffId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_StaffId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Bills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_StaffId",
                table: "Bills",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Staffs_StaffId",
                table: "Bills",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }
    }
}
