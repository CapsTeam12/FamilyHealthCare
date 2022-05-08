using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateColumnPrescriptionDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Prescriptions");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
