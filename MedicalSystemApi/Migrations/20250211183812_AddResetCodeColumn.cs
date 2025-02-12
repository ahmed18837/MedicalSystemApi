using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class AddResetCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "AspNetUsers");
        }
    }
}
