using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateusertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthChecks_Patients_PatientId1",
                table: "HealthChecks");

            migrationBuilder.DropIndex(
                name: "IX_HealthChecks_PatientId1",
                table: "HealthChecks");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "HealthChecks");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "HealthChecks");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "HealthChecks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_UserId",
                table: "HealthChecks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthChecks_AspNetUsers_UserId",
                table: "HealthChecks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthChecks_AspNetUsers_UserId",
                table: "HealthChecks");

            migrationBuilder.DropIndex(
                name: "IX_HealthChecks_UserId",
                table: "HealthChecks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HealthChecks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "HealthChecks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId1",
                table: "HealthChecks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_PatientId1",
                table: "HealthChecks",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthChecks_Patients_PatientId1",
                table: "HealthChecks",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
