using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateColumPresDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionDetails_Medicines_MedicineId",
                table: "PrescriptionDetails");

           

            migrationBuilder.DropColumn(
                name: "MedicineId",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<string>(
                name: "MedicineName",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicineName",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<int>(
                name: "MedicineId",
                table: "PrescriptionDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_MedicineId",
                table: "PrescriptionDetails",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionDetails_Medicines_MedicineId",
                table: "PrescriptionDetails",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
