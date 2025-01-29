using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBillMedicalTestAndMedicalTestTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.CommonSequence"),
                    TestName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillMedicalTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.CommonSequence"),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    MedicalTestId = table.Column<int>(type: "int", nullable: false),
                    TestCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillMedicalTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillMedicalTest_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillMedicalTest_MedicalTest_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillMedicalTest_BillId",
                table: "BillMedicalTest",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillMedicalTest_MedicalTestId",
                table: "BillMedicalTest",
                column: "MedicalTestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillMedicalTest");

            migrationBuilder.DropTable(
                name: "MedicalTest");
        }
    }
}
