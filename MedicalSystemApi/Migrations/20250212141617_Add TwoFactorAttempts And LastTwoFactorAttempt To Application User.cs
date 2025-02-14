using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactorAttemptsAndLastTwoFactorAttemptToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTwoFactorAttempt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TwoFactorAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTwoFactorAttempt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorAttempts",
                table: "AspNetUsers");
        }
    }
}
