using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdatePrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Prescriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordId",
                table: "Prescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionName",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "PrescriptionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "PrescriptionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoctorId",
                table: "Prescriptions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicalRecordId",
                table: "Prescriptions",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_MedicalRecords_MedicalRecordId",
                table: "Prescriptions",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_MedicalRecords_MedicalRecordId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_MedicalRecordId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicalRecordId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "PrescriptionName",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Appointment");
        }
    }
}
