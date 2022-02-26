using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addNewPropertiesDrnSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Join_Url",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Start_Url",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Join_Url",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Start_Url",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Doctors");
        }
    }
}
